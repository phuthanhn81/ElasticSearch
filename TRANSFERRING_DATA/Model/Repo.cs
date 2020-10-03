using ElasticsearchCRUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Model
{
    public class Repo
    {
        private Stopwatch stopwatch = new Stopwatch();

        public void Save()
        {
            IElasticsearchMappingResolver elasticsearchMappingResolver = new ElasticsearchMappingResolver();
            using (var elasticsearchContext = new ElasticsearchContext("http://localhost:9200/content", elasticsearchMappingResolver))
            {
                //elasticsearchContext.TraceProvider = new ConsoleTraceProvider();
                using (ElasticSearch db = new ElasticSearch())
                {
                    int pointer = 0;
                    const int interval = 100;
                    int length = db.Articles.Count();

                    while (pointer < length)
                    {
                        stopwatch.Start();
                        List<Article> collection = db.Articles.OrderBy(t => t.ID).Skip(pointer).Take(interval).ToList();
                        stopwatch.Stop();
                        Console.WriteLine("Time taken for select {0} AddressID: {1}", interval, stopwatch.Elapsed);
                        stopwatch.Reset();

                        foreach (Article item in collection)
                        {
                            elasticsearchContext.AddUpdateDocument(item, item.ID);
                            // elasticsearchContext.DeleteDocument<Article>(item.ID);
                            string t = "yes";
                        }

                        stopwatch.Start();
                        elasticsearchContext.SaveChanges();
                        stopwatch.Stop();
                        Console.WriteLine("Time taken to insert {0} AddressID documents: {1}", interval, stopwatch.Elapsed);
                        stopwatch.Reset();
                        pointer = pointer + interval;
                        Console.WriteLine("Transferred: {0} items", pointer);
                    }
                }
            }
        }
    }
}
