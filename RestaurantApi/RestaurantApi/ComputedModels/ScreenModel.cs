using Newtonsoft.Json;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApi.ComputedModels
{
    public class ScreenModel
    {
     
        [JsonSerializeAttribute]
        public string[][] Data { get; set; }
        [JsonSerializeAttribute]
        public string[] Categories { get; set; }
        [JsonSerializeAttribute]
        public string[] Tables { get; set; }

        public ScreenModel() { }
        public ScreenModel(DateTime date,Dao dao)
        {
            IList<Category> categoriesList = dao.List<Category>();
            string sql = $"Select * from FoodOrder where Date>='{date.ToString("yyyy-MM-dd")}' Order By OrderNumber";
            IList<FoodOrder> ordersList = dao.List<FoodOrder>(sql);
            int cols = categoriesList.Count+2;
            int rows = ordersList.Count;
            Tables = new string[rows];
            Data = new string[rows][];
            Category[] categoriesArray = categoriesList.OrderBy(category => category.Code).ToArray<Category>();
            Categories = new string[categoriesArray.Length];
            int ctr = 0;
            foreach(Category category in categoriesArray)
            {
                Categories[ctr++] = category.Name+"\n"+category.Code;
            }
            FoodOrder[] orders = ordersList.OrderBy(order => order.OrderNumber).ToArray<FoodOrder>();
            for (int row = 0; row < rows; row++)
            {
                FoodOrder o = orders[row];
                Data[row] = new string[cols];
                Tables[row] = o.Table.Number;
                Data[row][0] = o.Table.Number;
                Data[row][cols - 1] = o.Date.ToString();
                for (int col = 1; col < cols-1 ; col++)
                {
                    string msg = $"Table:'{o.Table.Number}':row:'{row}':col'{col}'";
                    Debug.WriteLine(msg);
                    OrderedItem[] items = o.OrderedItems.Where(item => item.Item.Category.Code == categoriesArray[col - 1].Code).ToArray<OrderedItem>();
                    if (items.Length == 0)
                    {
                        Data[row][col] = "0";
                    }
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (OrderedItem i in items)
                        {
                            string item = $"'{i.Item.Name}'\n'{(i.Quantity > 1 ? "" + i.Quantity : "")}' '{i.Item.Unit.Code}'";
                            builder.Append(item);
                            
                            //Data[row][col] += i.Item.Name + "\n" + (i.Quantity > 1 ? "" + i.Quantity : "") + " " + i.Item.Unit.Code;
                        }
                        Data[row][col] = builder.ToString();
                    }
                    
                }
            }
        }
        
        public virtual string ToJson()
        {

            return JsonConvert.SerializeObject(this, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
