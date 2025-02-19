﻿using Dapper;
using OSBookReviewWepApi.Models;
using OSDataAccessLibrary;

namespace OSBookReviewWepApi.Services
{
    public class DataClass : IDataClass
    {
        //TODO : Assign SQL Stored Procedure Calls 
        private readonly IDataAccess _data;

        public DataClass(IDataAccess data)
        {
            _data = data;
        }
        // add a single object 
        public async Task<bool> AddAsync(BookReview p)
        {
            return await InsertRecord(p);
        }
        // get indiviudal object type
        public async Task<BookReview> GetIndv(int bdid)
        {
            return await GetSingle(bdid);
        }
        // update one if required.
        public async Task<bool> UpdateAsync(BookReview p)
        {
            return await UpdateRecord(p);
        }
        // Update multiple objects at the same time
        public async Task<bool> UpdateAsync(List<BookReview> books)
        {
            return await UpdateList(books);
        }
        // gets a list of authors
        public async Task<List<Author>> GetListAsync()
        {
            return await GetAuthorList();
        }
        public async Task<List<Author>> GetListAsync(string name)
        {
            return await GetAuthorList(name);
        }
        // get list of object types with param requirements
        public async Task<List<BookReview>> GetListAsync(int aid)
        {
            return await GetBookList(aid);
        }

        // private methods to handle public calls

        // inserts a single record of an object 
        private async Task<bool> InsertRecord(BookReview book)
        {
            try
            {
                // add params
                DynamicParameters p = new();
                p.Add("@BDID", book.BDID);
                p.Add("@Userid", book.Userid);
                p.Add("@Rating", book.Rating);
                p.Add("@ReviewRemarks", book.ReviewRemarks);
                // call stored procedure
                string sql = "dbo.spInsertBookReview";
                // execute stored procedure via 
                var result = await _data.AddAsync(sql, p);

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // gets a single object passing a request of that object type in case of multiple param types
        // no current requirement
        private async Task<BookReview> GetSingle(int bdid)
        {
            // set of dynamic params depending on stored procedure params
            DynamicParameters p = new();
            p.Add("@bdid", bdid);

            // set the stored procedure string
            string sql = "dbo.spGetSingleBook";
            // return the result of the DAL call
            var res = await _data.GetIndvAsync<BookReview, dynamic>(sql, p);
            // check if book details exist if not return an empty book in event of book being deleted 
            if (res != null)
            {
                return res;
            }
            res = new BookReview();
            return res;
        }
        // updates a record of an object with the object passed for setting dynamic params
        // no current requirement
        private async Task<bool> UpdateRecord(BookReview p)
        {
            // set T type as param object

            // if not object required set dynamic params instead
            //DynamicParameters p = new();

            // set the stored procedure string
            string sql = "";
            // return the result of the DAL call
            return await _data.UpdateAsync(sql, p);

        }
        // updates multiple records
        private async Task<bool> UpdateList(List<BookReview> books)
        {
            try
            {
                int total = books.Count;
                int complete = 0;

                foreach (BookReview p in books)
                {
                    // call stored procedure
                    string sql = "";
                    // execute stored procedure via DAL 
                    var result = await _data.AddAsync(sql, p);

                    if (result)
                    {
                        complete += 1;
                    }
                }

                return total.Equals(complete);
            }
            catch (Exception)
            {
                return false;
            }
        }
        // gets a list of the object type depending on author id
        private async Task<List<BookReview>> GetBookList(int aid)
        {
            try
            {
                DynamicParameters p = new();
                // add params                
                p.Add("@AID", aid);
                // stored prodecure to be called
                string sql = "dbo.spGetAuthorBooks";

                List<BookReview> books = await _data.GetList<BookReview, dynamic>(sql, p);
                return books;
            }
            catch (Exception)
            {   // returns an empty list of type T in the event of exception being thrown
                List<BookReview> books = new();
                return books;
            }
        }
        // gets a list of authors
        private async Task<List<Author>> GetAuthorList()
        {
            try
            {
                DynamicParameters p = new();
                // add params if any

                // stored procedure to be called
                string sql = "dbo.spGetTop50Authors";

                List<Author> authors = await _data.GetList<Author, dynamic>(sql, p);
                return authors;
            }
            catch (Exception)
            {
                // returns an empty list of type T in the event of exception being thrown
                List<Author> authors = new();
                return authors;
            }
        }
        // gets a list of authors
        private async Task<List<BookReview>> GetAuthorList(int aid)
        {
            try
            {
                DynamicParameters p = new();
                // add params if any
                p.Add("@aid", aid);
                // stored procedure to be called
                string sql = "dbo.spGetAuthorsByID";

                List<BookReview> books = await _data.GetList<BookReview, dynamic>(sql, p);
                return books;
            }
            catch (Exception)
            {
                // returns an empty list of type T in the event of exception being thrown
                List<BookReview> books = new();
                return books;
            }
        }

        // gets a list of authors
        private async Task<List<Author>> GetAuthorList(string name)
        {
            try
            {
                DynamicParameters p = new();
                // add params if any
                p.Add("@aname", name);
                // stored procedure to be called
                string sql = "dbo.spGetAuthorsByName";

                List<Author> authors = await _data.GetList<Author, dynamic>(sql, p);
                return authors;
            }
            catch (Exception)
            {
                // returns an empty list of type T in the event of exception being thrown
                List<Author> authors = new();
                return authors;
            }

        }

    }
}
