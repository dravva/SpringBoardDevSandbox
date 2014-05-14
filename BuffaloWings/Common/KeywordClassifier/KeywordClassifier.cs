using Microsoft.Dldw.BuffaloWings.Common.KeywordClassifier.CHE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dldw.BuffaloWings.Common.KeywordClassifier
{
    using Microsoft.Dldw.BuffaloWings.Common.ApplicationCache;

    public class KeywordClassifier
    {
        #region private constructs
        
        private const string Region = "DWDLKeywordsCat";

        private readonly CHEFrontWebService cheFrontWebService= new CHEFrontWebService();

        private static readonly IAppCache applicationCache = new ApplicationCache(Region);

        private readonly CacheModel cacheModel = new CacheModel( new Duration( DateInterval.Hour,  1) );

        //todo: move to config

        // For English
        private int en_ModelId = 1;
        private int en_langId = 1033;

        // For Chinese
        private int zh_ModelId = 3;
        private int zh_langId = 2052;
        
        #endregion

        public IDictionary<string, KeywordCategories> GetTextClassification(IEnumerable<string> words, int topCategories, string market = "en-us")
        {
            if( null == words ) throw new ArgumentNullException( "words" );

            var keyWordCategoriesDic = new Dictionary<string, KeywordCategories>();

            foreach (var word in words)
            {
                
                // In order to reduce traffic to Ads prod env

                if (applicationCache.ContainsKey( word ))
                {
                    var categories = ( KeywordCategories )applicationCache.Get(word);
                    
                    keyWordCategoriesDic.Add(word, categories);
                }
                else
                {

                    ClassifyStatus cs;

                    if( market == "en-us" )
                    {
                        cs = cheFrontWebService.Classify_GetResults( word, en_ModelId, en_langId, topCategories );
                    }
                    else if( market == "en-cn" )
                    {
                        cs = cheFrontWebService.Classify_GetResults( word, zh_ModelId, zh_langId, topCategories );
                    }
                    else
                    {
                        throw new ArgumentException( "Unsupported language" );
                    }

                    if( cs.IsSucceeded )
                    {
                        var categories = new KeywordCategories { Categories = cs.results.Select( result => result.CategoryName ).ToList() };

                        keyWordCategoriesDic.Add( word, categories );

                        applicationCache.Set( word, categories, cacheModel );
                    }
                }

            }

            return keyWordCategoriesDic;

        }


        public IDictionary<string, KeywordCategories> GetTextClassificationFromCache(IEnumerable<string> words, int topCategories, string market = "en-us")
        {
            if (null == words) throw new ArgumentNullException("words");

            var keyWordCategoriesDic = new Dictionary<string, KeywordCategories>();

            foreach (var word in words)
            {

                // In order to reduce traffic to Ads prod env

                if (applicationCache.ContainsKey(word))
                {
                    var categories = (KeywordCategories)applicationCache.Get(word);

                    keyWordCategoriesDic.Add(word, categories);
                }
               
            }

            return keyWordCategoriesDic;

        }


    }
}
