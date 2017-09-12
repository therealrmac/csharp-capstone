using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChatItUp.Data;
using ChatItUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;


namespace ChatItUp.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
                {
                    if(context.Category.Any())
                    {
                        return; //db has been seeded already
                    }
                var Categories = new Category[]
                {
                        new Category
                        {
                            title= "Entertainment",
                            image= "https://www.shareicon.net/download/2015/09/27/108077_film_512x512.png"
                        },
                        new Category
                        {
                            title= "PC",
                            image= "http://www.iconsfind.com/wp-content/uploads/2017/03/20170320_58cf7da05fcf9.png"
                        },
                        new Category
                        {
                            title= "Sports",
                            image= "https://www.shareicon.net/download/2015/09/23/105826_cup_512x512.png"
                        },
                        new Category
                        {
                            title= "Music",
                            image= "https://www.shareicon.net/download/2015/09/09/98326_film_512x512.png"
                        },
                        new Category
                        {
                            title= "Photography",
                            image= "https://www.shareicon.net/download/2015/09/28/108380_camera_512x512.png"
                        },
                        new Category
                        {
                            title= "Books",
                            image= "https://www.shareicon.net/download/2016/11/14/852123_book_512x512.png"
                        },
                        new Category
                        {
                            title= "Food",
                            image= "https://www.shareicon.net/download/2015/09/27/108025_food_512x512.png"
                        },
                        new Category
                        {
                            title= "Firearms",
                            image= "https://www.shareicon.net/download/2015/10/19/658751_gun_512x512.png"
                        },
                        new Category
                        {
                            title= "Video Games",
                            image= "https://cdn4.iconfinder.com/data/icons/glyphlibrary-one/100/controller-xbox-512.png"
                        }
                    };
                foreach(Category x in Categories)
                {
                    context.Category.Add(x);
                }
                context.SaveChanges();

                var forum = new Forum[]
                {
                    new Forum
                    {
                        ThreadTitles= "Movies",
                        CategoryId= 1
                    },
                    new Forum
                    {
                        ThreadTitles= "Television",
                        CategoryId= 1
                    },
                    new Forum
                    {
                        ThreadTitles= "Broadway Plays",
                        CategoryId= 1
                    },
                    new Forum
                    {
                        ThreadTitles= "Comedians",
                        CategoryId= 1
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "Components",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "Software",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "Custom Build",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "Tablets",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "Support",
                        CategoryId= 2
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Football",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Basketball",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Baseball",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "E Sports",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Tennis",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Golf",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "Soccer / Futbol",
                        CategoryId= 3
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Rock",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Rap",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Pop",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Country",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Metal",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Punk",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Jazz",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Ska",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "Funk",
                        CategoryId= 4
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Equpiment",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Photo Share",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Lighting",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Development",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Locations",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "Video Recording",
                        CategoryId= 5
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 6
                    },
                    new Forum
                    {
                        ThreadTitles= "Authors",
                        CategoryId= 6
                    },
                    new Forum
                    {
                        ThreadTitles= "Favorite Read",
                        CategoryId= 6
                    },
                    new Forum
                    {
                        ThreadTitles= "Self Written",
                        CategoryId= 6
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "Comfort",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "Recipies",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "Ethnic Dishes",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "Presentation",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "Seasonal Cooking",
                        CategoryId= 7
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Sidearms / Handguns",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Rifle",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Skeet Shoot",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Customization",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Repair",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Ammunition Reloading",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "Home Defense",
                        CategoryId= 8
                    },
                    new Forum
                    {
                        ThreadTitles= "General",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "League of Legends",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "Destiny 1 & 2",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "CS:GO",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "Overwatch",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "HearthStone",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "First Person Shooter",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "Real Time Strategy",
                        CategoryId= 9
                    },
                    new Forum
                    {
                        ThreadTitles= "MMORPG",
                        CategoryId= 9
                    },

                };
                foreach (Forum x in forum)
                {
                    context.Forum.Add(x);
                }
                context.SaveChanges();
            }
        }
    }
}
