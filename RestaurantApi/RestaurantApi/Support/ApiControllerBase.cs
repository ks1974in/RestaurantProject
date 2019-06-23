using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NHibernate;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApi.Support
{
    public class ApiControllerBase : ControllerBase, IApiController
    {
        protected readonly Dao dao;

        protected ApiControllerBase(ISessionFactory sessionFactory)
        {
            dao = new Dao(sessionFactory);
        }
        public enum HttpAction { Get, Post, Put, Patch, Delete }
        protected IActionResult HandleGetException(Exception e, string message = null)
        {
            return HandleException(e, HttpAction.Get, message);
        }
        protected IActionResult HandlePostException(Exception e, string message = null)
        {
            return HandleException(e, HttpAction.Post, message);
        }
        protected IActionResult HandlePutException(Exception e, string message = null)
        {
            return HandleException(e, HttpAction.Put, message);
        }
        protected IActionResult HandlePatchException(Exception e, string message = null)
        {
            return HandleException(e, HttpAction.Patch, message);
        }
        protected IActionResult HandleDeleteException(Exception e, string message = null)
        {
            return HandleException(e, HttpAction.Delete, message);
        }


        protected IActionResult HandleException(Exception e, HttpAction action, string message)
        {
            ApiException error;

            //Object is already present in database, in a Post call. Use put instead
            if (e is ObjectExistsException)
            {
                error = new ApiException(action, ApiException.ERROR_CODE_OBJECT_EXISTS, e.Message + ApiException.ERROR_MSG_OBJECT_EXISTS);
                return BadRequest(error);
            }
            /*Id and object.id in put mismatch*/
            else if (e is IdMismatchException)
            {
                error = new ApiException(action, ApiException.ERROR_CODE_ID_MISMATCH, e.Message + ApiException.ERROR_MSG_ID_MISMATCH);
                LogException(StatusCodes.Status400BadRequest, e, error);
                return BadRequest(error);
            }
            else if (e is ObjectNotFoundException)
            {
                error = new ApiException(action, ApiException.ERROR_CODE_NOT_FOUND, e.Message + ApiException.ERROR_MSG_NHIBERNATE);
                if (action == HttpAction.Get)
                {

                    LogException(StatusCodes.Status404NotFound, e, error);
                    return NotFound(error);
                }
                else
                {
                    LogException(StatusCodes.Status400BadRequest, e, error);
                    return BadRequest(error);
                }

            }
            else
            {
                if (e.InnerException != null)
                {
                    error = new ApiException(action, ApiException.ERROR_CODE_NHIBERNATE, e.Message, new ApiException(action, ApiException.ERROR_CODE_NHIBERNATE_INNER, e.InnerException.Message, null), ToJson(e));
                    LogException(StatusCodes.Status500InternalServerError, e, error);
                }
                else
                {
                    error = new ApiException(action, ApiException.ERROR_CODE_NHIBERNATE, message, new ApiException(action, ApiException.ERROR_CODE_NHIBERNATE, e.Message, null), ToJson(e));
                    LogException(StatusCodes.Status500InternalServerError, e, error);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }




        }
        private void LogException(int statusCode, Exception e, ApiException error)
        {
            string msg = TranslateStatusCode(statusCode);
            Debug.WriteLine("Status Code:" + statusCode + ":" + msg);
            Debug.WriteLine(e.Message);
            Debug.WriteLine(e.StackTrace);
            Debug.WriteLine(JsonConvert.SerializeObject(e));
            Debug.WriteLine(JsonConvert.SerializeObject(error));
        }

        protected string TranslateStatusCode(int statusCode)
        {
            string msg;
            switch (statusCode)
            {
                case StatusCodes.Status200OK: msg = "OK"; break;
                case StatusCodes.Status400BadRequest: msg = "Bad Request"; break;
                default: msg = "Internal Server Error"; break;

            }
            return msg;
        }
        private string ToJson(Exception e)
        {
            return JsonConvert.SerializeObject(e, Formatting.Indented);
        }
        protected void ValidateIds<T>(Guid id, IEntity entity)
        {
            if (id != entity.Id) throw new IdMismatchException(id, typeof(T));
        }
        protected void ValidateEntityNotInDatabase<T>(IEntity entity)
        {
            T dbEntity = GetEntity<T>(entity.Id);
            if (dbEntity != null) throw new ObjectExistsException(entity);
        }
        protected void ValidateEntityInDatabase<T>(IEntity entity)
        {
           _ = LoadEntityById<T>(entity.Id);
        }
        protected void ValidateEntityInDatabase<T>(Guid id)
        {
             _ = LoadEntityById<T>(id);
        }
       
        protected T GetEntity<T>(Guid id)
        {
        T entity = dao.Get<T>(id);
       
        return entity;
        }
        protected T LoadEntityByKeyValue<T>(string key,string value)
        {
            string sql=$"Select * from  [{typeof(T).Name}] where [{key}]='{value}'";
            Debug.WriteLine(sql);
            try
            {
                T entity = dao.Query<T>(sql);
                return entity;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ObjectNotFoundByKeyValueException(key, value, typeof(T));
            }
        }
        protected IList<T> ListEntitiesByKeyValue<T>(string key, Guid value)
        {
            string sql = $"Select * from  [{typeof(T).Name}] where [{key}]='{value}'";
            Debug.WriteLine(sql);
            try
            {
                IList<T> list = dao.List<T>(sql);
                return list;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ObjectNotFoundByKeyValueException(key, value.ToString(), typeof(T));
            }
        }
        protected T LoadEntityByKeyValue<T>(string key, Guid value)
        {
            string sql = $"Select * from  [{typeof(T).Name}] where [{key}]='{value}'";
            Debug.WriteLine(sql);
            try
            {
                T entity = dao.Query<T>(sql);
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ObjectNotFoundByKeyValueException(key, value.ToString(), typeof(T));
            }
        }
        protected T LoadEntityById<T>(Guid id)
        {
            try
            {
                T entity = dao.Load<T>(id);
                return entity;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ObjectNotFoundByIdException(id,typeof(T));
            }
        }


    }
}
