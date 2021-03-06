﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.MT.UserInterestAggregator
{
    public static class InterestCategoryList
    {


        public static Dictionary<String, String> CategoryMap = new Dictionary<String, String>(StringComparer.InvariantCultureIgnoreCase);

        static InterestCategoryList()
        {

            CategoryMap.Add("Actor/Director", "Movies and TV Shows");
            CategoryMap.Add("Aerospace/Defense", "Others");
            CategoryMap.Add("Airport", "Travel");
            CategoryMap.Add("Album", "Music");
            CategoryMap.Add("Amateur Sports Team", "Sports");
            CategoryMap.Add("App Page", "Apps");
            CategoryMap.Add("Appliances", "Household");
            CategoryMap.Add("Artist", "Movies and TV Shows");
            CategoryMap.Add("Arts/Entertainment/Nightlife", "Entertainment");
            CategoryMap.Add("Arts/Humanities Website", "Websites");
            CategoryMap.Add("Athlete", "Sports");
            CategoryMap.Add("Attractions/Things to Do", "Travel");
            CategoryMap.Add("Author", "Books");
            CategoryMap.Add("Automobiles and Parts", "Autos");
            CategoryMap.Add("Automotive", "Autos");
            CategoryMap.Add("Baby Goods/Kids Goods", "Household");
            CategoryMap.Add("Bags/Luggage", "Household");
            CategoryMap.Add("Bank/Financial Institution", "Organizations");
            CategoryMap.Add("Bank/Financial Services", "Organizations");
            CategoryMap.Add("Bar", "Entertainment");
            CategoryMap.Add("Biotechnology", "Others");
            CategoryMap.Add("Board Game", "Sports");
            CategoryMap.Add("Book", "Books");
            CategoryMap.Add("Book Series", "Books");
            CategoryMap.Add("Book Store", "Books");
            CategoryMap.Add("Building Materials", "Household");
            CategoryMap.Add("Business Person", "Business");
            CategoryMap.Add("Business Services", "Business");
            CategoryMap.Add("Business/Economy Website", "Websites");
            CategoryMap.Add("Camera/Photo", "Household");
            CategoryMap.Add("Cars", "Autos");
            CategoryMap.Add("Cause", "Others");
            CategoryMap.Add("Chef", "Food and Beverages");
            CategoryMap.Add("Chemicals", "Others");
            CategoryMap.Add("Church/Religious Organization", "Religion");
            CategoryMap.Add("Clothing", "Fashion");
            CategoryMap.Add("Club", "Entertainment");
            CategoryMap.Add("Coach", "Sports");
            CategoryMap.Add("Comedian", "Movies and TV Shows");
            CategoryMap.Add("Commercial Equipment", "Electronics");
            CategoryMap.Add("Community", "Others");
            CategoryMap.Add("Community Organization", "Organizations");
            CategoryMap.Add("Community/Government", "Organizations");
            CategoryMap.Add("Company", "Profession");
            CategoryMap.Add("Computers", " IT");
            CategoryMap.Add("Computers/Internet Website", "Websites");
            CategoryMap.Add("Computers/Technology", "IT");
            CategoryMap.Add("Concert Tour", "Entertainment");
            CategoryMap.Add("Concert Venue", "Entertainment");
            CategoryMap.Add("Consulting/Business Services", "Profession");
            CategoryMap.Add("Dancer", "Entertainment");
            CategoryMap.Add("Doctor", "Personal Care");
            CategoryMap.Add("Drugs", "Personal Care");
            CategoryMap.Add("Education", "Education");
            CategoryMap.Add("Education Website", "Websites");
            CategoryMap.Add("Electronics", "Electronics");
            CategoryMap.Add("Energy/Utility", "Others");
            CategoryMap.Add("Engineering/Construction", "Others");
            CategoryMap.Add("Entertainer", "Entertainment");
            CategoryMap.Add("Entertainment Website", "Websites");
            CategoryMap.Add("Episode", "Movies and TV Shows");
            CategoryMap.Add("Event Planning/Event Services", "Others");
            CategoryMap.Add("Farming/Agriculture", "Others");
            CategoryMap.Add("Fictional Character", "Celebrity");
            CategoryMap.Add("Food/Beverages", "Food and Beverages");
            CategoryMap.Add("Food/Grocery", "Food and Beverages");
            CategoryMap.Add("Furniture", "Household");
            CategoryMap.Add("Games/Toys", "Sports");
            CategoryMap.Add("Government Official", "Celebrity");
            CategoryMap.Add("Government Organization", "Organizations");
            CategoryMap.Add("Government Website", "Websites");
            CategoryMap.Add("Health/Beauty", "Personal Care");
            CategoryMap.Add("Health/Medical/Pharmaceuticals", "Personal Care");
            CategoryMap.Add("Health/Medical/Pharmacy", "Personal Care");
            CategoryMap.Add("Health/Wellness Website", "Websites");
            CategoryMap.Add("Home Decor", "Household");
            CategoryMap.Add("Home Improvement", "Household");
            CategoryMap.Add("Home/Garden Website", "Websites");
            CategoryMap.Add("Hospital/Clinic", "Personal Care");
            CategoryMap.Add("Hotel", "Travel");
            CategoryMap.Add("Household Supplies", "Household");
            CategoryMap.Add("Industrials", "Others");
            CategoryMap.Add("Insurance Company", "Personal Care");
            CategoryMap.Add("Internet/Software", "IT");
            CategoryMap.Add("Jewelry/Watches", "Fashion");
            CategoryMap.Add("Journalist", "Profession");
            CategoryMap.Add("Just For Fun", "Others");
            CategoryMap.Add("Kitchen/Cooking", "Household");
            CategoryMap.Add("Landmark", "Travel");
            CategoryMap.Add("Lawyer", "Profession");
            CategoryMap.Add("Legal/Law", "Others");
            CategoryMap.Add("Library", "Books");
            CategoryMap.Add("Literary Editor", "Books");
            CategoryMap.Add("Local Business", "Business");
            CategoryMap.Add("Local/Travel Website", "Websites");
            CategoryMap.Add("Magazine", "Books");
            CategoryMap.Add("Media/News/Publishing", "Entertainment");
            CategoryMap.Add("Mining/Materials", "Others");
            CategoryMap.Add("Monarch", "Others");
            CategoryMap.Add("Movie", "Movies and TV Shows");
            CategoryMap.Add("Movie Character", "Movies and TV Shows");
            CategoryMap.Add("Movie Theater", "Movies and TV Shows");
            CategoryMap.Add("Museum/Art Gallery", "Travel");
            CategoryMap.Add("Music Award", "Music");
            CategoryMap.Add("Music Chart", "Music");
            CategoryMap.Add("Music Video", "Music");
            CategoryMap.Add("Musician/band", "Music");
            CategoryMap.Add("News Personality", "Celebrity");
            CategoryMap.Add("News/Media Website", "Websites");
            CategoryMap.Add("Non-Governmental Organization (NGO)", "Organizations");
            CategoryMap.Add("Non-Profit Organization", "Organizations");
            CategoryMap.Add("Office Supplies", "Office");
            CategoryMap.Add("Organization", "Organizations");
            CategoryMap.Add("Outdoor Gear/Sporting Goods", "Household");
            CategoryMap.Add("Patio/Garden", "Household");
            CategoryMap.Add("Personal Blog", "Others");
            CategoryMap.Add("Personal Website", "Websites");
            CategoryMap.Add("Pet", "Others");
            CategoryMap.Add("Pet Supplies", "Household");
            CategoryMap.Add("Phone/Tablet", "IT");
            CategoryMap.Add("Political Organization", "Organizations");
            CategoryMap.Add("Political Party", "Others");
            CategoryMap.Add("Politician", "Profession");
            CategoryMap.Add("Producer", "Movies and TV Shows");
            CategoryMap.Add("Product/Service", "Profession");
            CategoryMap.Add("Professional Services", "Others");
            CategoryMap.Add("Professional Sports Team", "Sports");
            CategoryMap.Add("Public Figure", "Celebrity");
            CategoryMap.Add("Public Places", "Travel");
            CategoryMap.Add("Publisher", "Books");
            CategoryMap.Add("Radio Station", "Music");
            CategoryMap.Add("Real Estate", "Others");
            CategoryMap.Add("Record Label", "Others");
            CategoryMap.Add("Recreation/Sports Website", "Websites");
            CategoryMap.Add("Reference Website", "Websites");
            CategoryMap.Add("Regional Website", "Websites");
            CategoryMap.Add("Restaurant/cafe", "Food and Beverages");
            CategoryMap.Add("Retail and Consumer Merchandise", "Organizations");
            CategoryMap.Add("School", "Education");
            CategoryMap.Add("School Sports Team", "Sports");
            CategoryMap.Add("Science Website", "Websites");
            CategoryMap.Add("Shopping/Retail", "Others");
            CategoryMap.Add("Small Business", "Business");
            CategoryMap.Add("Society/Culture Website", "Websites");
            CategoryMap.Add("Software", "IT");
            CategoryMap.Add("Song", "Music");
            CategoryMap.Add("Spas/Beauty/Personal Care", "Personal Care");
            CategoryMap.Add("Sports Event", "Sports");
            CategoryMap.Add("Sports League", "Sports");
            CategoryMap.Add("Sports Venue", "Sports");
            CategoryMap.Add("Sports/Recreation/Activities", "Sports");
            CategoryMap.Add("Studio", "Movies and TV Shows");
            CategoryMap.Add("Teacher", "Education");
            CategoryMap.Add("Teens/Kids Website", "Websites");
            CategoryMap.Add("Telecommunication", "IT");
            CategoryMap.Add("Tools/Equipment", "Electronics");
            CategoryMap.Add("Tours/Sightseeing", "Travel");
            CategoryMap.Add("Transport/Freight", "Travel");
            CategoryMap.Add("Transportation", "Travel");
            CategoryMap.Add("Travel/Leisure", "Travel");
            CategoryMap.Add("TV Channel", "Movies and TV Shows");
            CategoryMap.Add("TV Network", "Movies and TV Shows");
            CategoryMap.Add("TV Show", "Movies and TV Shows");
            CategoryMap.Add("TV/Movie Award", "Movies and TV Shows");
            CategoryMap.Add("University", "Education");
            CategoryMap.Add("Video Game", "Sports");
            CategoryMap.Add("Vitamins/Supplements", "Personal Care");
            CategoryMap.Add("Website", "IT");
            CategoryMap.Add("Wine/Spirits", "Food and Beverages");
            CategoryMap.Add("Writer", "Movies and TV Shows");
            CategoryMap.Add("Books", "Books");
            CategoryMap.Add("Music", "Music");
            CategoryMap.Add("Movies", "Movies and TV Shows");
            CategoryMap.Add("Online Videos", "Movies and TV Shows");
            CategoryMap.Add("TV Shows", "Movies and TV Shows");

        }



    }

}
