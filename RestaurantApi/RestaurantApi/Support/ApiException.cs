using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static RestaurantApi.Support.ApiControllerBase;

namespace RestaurantApi.Support
{

    public class GenericException : Exception
    {
        public GenericException() { }
        public GenericException(string message) : base(message) { }
    }

    public class ObjectNotFoundException : GenericException
    {
        public ObjectNotFoundException(string message) : base(message) { }
    }
    public class ObjectNotFoundByIdException:ObjectNotFoundException{
        public ObjectNotFoundByIdException(Guid id, Type type) : base($"Object '{type.Name}' with Id '{id}' does not exist in the database. "){}
    }
    public class ObjectNotFoundByKeyValueException : ObjectNotFoundException
    {
        public ObjectNotFoundByKeyValueException(string key,string value, Type type) : base($"Object '{type.Name}' with key '{key}' and value '{value}' does not exist in the database. ") { }
    }
    public class ObjectExistsException : GenericException
    {
        public ObjectExistsException(IEntity entity) : base($"Object '{entity.GetType().Name}' with Id '{entity.Id}'already exists in the database. ") { }
    }
    public class IdMismatchException : GenericException
    {

        public IdMismatchException(Guid id,Type type) : base($"Parameter id '{id}' does not match Id of object '{type.Name}'.") { }
    }
    public class ApiException : Exception
    {


        public static readonly  int ERROR_CODE_INTERNAL_SERVER_ERROR = 500;
        
        public static readonly  int ERROR_CODE_NOT_FOUND = 404;


        public static readonly  int ERROR_CODE_BAD_REQUEST = 1100;
        public static readonly  int ERROR_CODE_NHIBERNATE = 1000;
        public static readonly  int ERROR_CODE_NHIBERNATE_INNER = 1010;

        /*Custom Error Messages to handle specific conditions*/

        /*Id and entity.Id mismatch in Put/Patch*/
        public static readonly int ERROR_CODE_ID_MISMATCH = 1020;
        public static readonly int ERROR_CODE_OBJECT_EXISTS = 1030;


        public static readonly  string ERROR_MSG_INTERNAL_SERVER_ERROR = "Internal Server Error. ";
        public static readonly  string ERROR_MSG_NHIBERNATE = "NHibernate Error. ";


        /*Custom Error Messages to handle specific conditions*/

        /*Id and entity.Id mismatch in Put/Patch*/

        public static readonly string ERROR_MSG_ID_MISMATCH = "Ids do not match. ";
        public static readonly string ERROR_MSG_OBJECT_EXISTS = "Please use Put or Patch instead of Post. ";


        public ApiException(HttpAction action, int code, string message)
        {
            Action = action.ToString();
            Code = code;
            Message = message;
        }

        public ApiException(HttpAction action, int code, ApiException innerException)
        {
            Action = action.ToString();
            Code = code;
            InnerError= innerException;
        }

        public ApiException(HttpAction action,int code, string message,string rawError)
        {
            Action = action.ToString();
            Code =code;
            Message= message;
            RawError = rawError;
            
        }

      
        public ApiException(HttpAction action, int code, string message, ApiException innerError,string rawError) : this(action, code, message,rawError)
        {
            InnerError = innerError;
        }
        


        public ApiException() { }


        public ApiException(string message) : base(message) { }

        [JsonSerializeAttribute]
        public int Code { get; set; }

        [JsonSerializeAttribute]
        public ApiException InnerError { get; set; }
        [JsonSerializeAttribute]
        public ApiException[] Details { get; set; }
        [JsonSerializeAttribute]
        public new string Message { get; set; }
        [JsonSerializeAttribute]
        public string Action { get; set; }
        [JsonSerializeAttribute]
        public string RawError { get; set; }
    }
}
