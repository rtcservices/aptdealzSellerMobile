﻿using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        CategoryAPI categoryAPI = new CategoryAPI();
        List<Category> categories = new List<Category>();
        List<SubCategory> subCategories = new List<SubCategory>();

        public async Task<List<Category>> GetCategory()
        {
            try
            {
                categories = await categoryAPI.GetCategory();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/GetCategory: " + ex.Message);
            }
            return categories;
        }

        public async Task<List<SubCategory>> GetSubCategory(string CategortyId)
        {
            try
            {
                subCategories = await categoryAPI.GetSubCategory(CategortyId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/GetSubCategory: " + ex.Message);
            }

            return subCategories;
        }

        public async Task<List<Category>> CreateCategory(string OtherCategory)
        {
            try
            {
                var mResponse = await categoryAPI.CreateCategory(OtherCategory);
                if (mResponse != null && mResponse.Succeeded)
                {
                    await GetCategory();
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/CreateCategory: " + ex.Message);
            }
            return categories;
        }

        public async Task<List<SubCategory>> CreateSubCategory(string OtherSubCategory, string categoryId)
        {
            try
            {
                var mResponse = await categoryAPI.CreateSubCategory(OtherSubCategory, categoryId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    await GetSubCategory(categoryId);
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/CreateSubCategory: " + ex.Message);
            }
            return subCategories;
        }
    }
}
