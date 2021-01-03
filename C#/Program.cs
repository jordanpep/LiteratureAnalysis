using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
using WordCloud;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {

                    // 定义接口
        protected static IMongoDatabase _database;
        // 定义客户端
        protected static IMongoClient _client;

        public static bool IsNumber(string s)
        {
            String[] s1 = s.Split();
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }
        static void Main(string[] args)
        {
            // 定义要查询的集合名称
            const string collectionName = "2020";
            // 读取连接字符串
            string strCon = ConfigurationManager.ConnectionStrings["mongodbConn"].ConnectionString;
            var mongoUrl = new MongoUrlBuilder(strCon);
            
            // 获取数据库名称
            string databaseName = mongoUrl.DatabaseName;
            // 创建并实例化客户端
            _client = new MongoClient(mongoUrl.ToMongoUrl());
            //  根据数据库名称实例化数据库
            _database = _client.GetDatabase(databaseName);
            // 根据集合名称获取集合
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var filter = new BsonDocument();
            // 查询集合中的文档
            var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
            // 循环遍历输出

          
            string file0= "../../stopwords.txt";

            string file = "../../csv/outyear.csv"; //年份论文输出                
            string file2 = "../../csv/outmonth.csv";//月份论文输出
            string file3 = "../../csv/outquarter.csv"; //季度论文输出
            

                
                String file1 = "";//词频输出
                bool num = false;
                bool fyear = false;
                bool fmonth = false;
                String yearstring = "";
                String monthstring = "";
                String cipin = "";
                Console.WriteLine("按年份-->1  按月份-->2   统计论文的数量-->3");
                String ym = Console.ReadLine().ToString();            
                if (ym == "1")
                {
                    fyear = true;
                    Console.WriteLine("请输输入年份");
                    yearstring = Console.ReadLine().ToString();                   
                    Console.WriteLine("请输入词频选项 1 简述 2 关键词 3方法");
                    cipin = Console.ReadLine();                    
                    if (cipin == "1")
                    {
                        file1 = "../../csv/" + yearstring + "abstract.csv";
                    }
                    else if (cipin == "2")
                    {
                        file1 = "../../csv/" + yearstring + "keyword.csv";
                    }
                    else if (cipin == "3")
                    {
                        file1 = "../../csv/" + yearstring + "method.csv";
                    }
                }
                else
                {
                    if (ym == "2")
                    {
                        fmonth = true;
                        Console.WriteLine("请输输入月份");
                        monthstring = Console.ReadLine().ToString();
                        
                        Console.WriteLine("请输入词频选项 1 简述 2 关键词 3方法");
                        cipin = Console.ReadLine();
                       
                        if (cipin == "1")
                        {
                            file1 = "../../csv/" + monthstring + "abstract.csv";
                        }
                        else if (cipin == "2")
                        {
                            file1 = "../../csv/" + monthstring + "keyword.csv";
                        }
                        else if (cipin == "3")
                        {
                            file1 = "../../csv/" + monthstring + "method.csv";
                        }
                    }
                    else
                    {
                        if (ym == "3")
                        {
                        num = true;
                        }
                    }
            }




                FileStream stopwords = new FileStream(file0, FileMode.Open);
                FileStream fs = new FileStream(file, FileMode.Create);
                FileStream fs1 = new FileStream(file1, FileMode.Create);
                FileStream fs2 = new FileStream(file2, FileMode.Create);
                FileStream fs3 = new FileStream(file3, FileMode.Create);

                StreamReader rd = new StreamReader(stopwords);
                StreamWriter sw = new StreamWriter(fs);
                StreamWriter sw1 = new StreamWriter(fs1);
                StreamWriter sw2 = new StreamWriter(fs2);
                StreamWriter sw3 = new StreamWriter(fs3);

                Hashtable htyear = new Hashtable(); //按年分论文
                Hashtable htmonth = new Hashtable();  //按月分论文
                Hashtable htquarter = new Hashtable(); //按季度分论文
                Hashtable ht1 = new Hashtable(); //处理文字

                list.ForEach(p =>
                {

                    String year = p["year"].ToString();
                    String month = p["month"].ToString();
                    String[] absdeal = { "" };
                    if (cipin == "1")
                    {
                        absdeal = p["abstract"].ToString().Split(' ', ',', '.', ':', '(', ')', '%', '=', ';');
                    }
                    //导入简述
                    if (cipin == "2")
                    {
                        if (p.Contains("Keywords"))
                        {
                            absdeal = p["Keywords"].ToString().Split(',', '.', ':', '(', ')', '%', '=', ';');
                        }
                    }
                    //导入关键词
                    if (cipin == "3")
                    {
                        if (p.Contains("Methods"))
                        {
                            absdeal = p["Methods"].ToString().Split(' ', ',', '.', ':', '(', ')', '%', '=', ';');
                        }
                    }
                    //导入方法
                    if(num)
                    {                                
                        if (htyear.ContainsKey(year))
                        {
                            htyear[year] = (int)htyear[year] + 1;
                        }
                        else
                        {
                            htyear.Add(year, 1);
                        }
                        //统计年份的论文
                        if (htmonth.ContainsKey(month))
                        {
                            htmonth[month] = (int)htmonth[month] + 1;
                        }
                        else
                        {
                            htmonth.Add(month, 1);
                        }
                        //统计月份的论文
                        String quarter = year.ToString();
                        switch (month.ToString())
                        {
                            case "1": quarter += "fist quarter";break;
                            case "2": quarter += "fist quarter"; break;
                            case "3": quarter += "fist quarter"; break;
                            case "4": quarter += "second quarter"; break;
                            case "5": quarter += "second quarter"; break;
                            case "6": quarter += "second quarter"; break;
                            case "7": quarter += "third quarter"; break;
                            case "8": quarter += "third quarter"; break;
                            case "9": quarter += "third quarter"; break;
                            case "10": quarter += "forth quarter"; break;
                            case "11": quarter += "forth quarter"; break;
                            case "12": quarter += "forth quarter"; break;
                            default:break;
                        }
                        if(htquarter.ContainsKey(quarter))
                        {
                            htquarter[quarter] = (int)htquarter[quarter] + 1;
                        }
                        else
                        {
                            htquarter.Add(quarter, 1);
                        }
                                //统计季度的论文
                }
                    //统计论文数量
                    if (fyear)
                    {
                        if (yearstring == "all")
                        {
                            foreach (var a in absdeal)
                            {
                                if (ht1.ContainsKey(a))
                                {
                                    ht1[a] = (int)ht1[a] + 1;
                                }
                                else
                                {
                                    ht1.Add(a, 1);
                                }
                            }
                        }
                        else
                        {
                            if (yearstring == year)
                            {
                                foreach (var a in absdeal)
                                {
                                    if (ht1.ContainsKey(a))
                                    {
                                        ht1[a] = (int)ht1[a] + 1;
                                    }
                                    else
                                    {
                                        ht1.Add(a, 1);
                                    }
                                }
                            }
                        }
                    }
                    //按照年生成词频
                    if (fmonth)
                    {
                        if (monthstring == month)
                        {
                            foreach (var a in absdeal)
                            {
                                if (ht1.ContainsKey(a))
                                {
                                    ht1[a] = (int)ht1[a] + 1;
                                }
                                else
                                {
                                    ht1.Add(a, 1);
                                }
                            }
                        }
                    }
                    //按照月生成词频

            });
                //导入数据到链表
                ICollection key = ht1.Keys;
                String line = "";
                ArrayList newone = new ArrayList(key);
                while ((line = rd.ReadLine()) != null)
                {
                    foreach (var k in key)
                    {
                        //Console.WriteLine(k);
                        if (k.ToString().Equals(line, StringComparison.OrdinalIgnoreCase))
                        {                            
                            newone.Remove(k);
                        }
                    }
                }
                Console.WriteLine("过滤停用词完成");
                for (int k = 0; k < newone.Count; k++)
                {
                    if (IsNumber(newone[k].ToString()))
                    {
                        newone.Remove(newone[k]);
                        //Console.WriteLine("remove: " + newone[k]);
                    }
                }
                Console.WriteLine("过滤数字完成");
                foreach (var k in newone)
                {
                    if ((int)ht1[k] > 1 && ht1[k].ToString() != "")
                    {
                        sw1.WriteLine(k + "," + ht1[k]);
                    }
                }


                ICollection yearkey = htyear.Keys;
                foreach (var k in yearkey)
                {
                    sw.WriteLine(k + "," + htyear[k]);
                }
                ICollection monthkey = htmonth.Keys;
                foreach (var k in monthkey)
                {
                    sw2.WriteLine(k + "," + htmonth[k]);
                }
                ICollection quarterkey = htquarter.Keys;
                foreach (var k in quarterkey)
                {
                    sw3.WriteLine(k + "," + htquarter[k]);
                }


                Console.WriteLine("程序完成");
               
                rd.Close();
                sw.Close();
                sw1.Close();
                sw2.Close();
                sw3.Close();
                fs.Close();
                fs1.Close();
                fs2.Close();
                fs3.Close();
            
        }
    
    }
}
