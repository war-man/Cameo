using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.Models;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.Controllers
{
    public class TalentScriptController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ISocialAreaService SocialAreaService;
        private readonly ICategoryService CategoryService;

        private List<SocialArea> socialAreas = new List<SocialArea>();
        private List<Category> categories = new List<Category>();

        public TalentScriptController(
            ITalentService talentService,
            ISocialAreaService socialAreaService,
            ICategoryService categoryService)
        {
            TalentService = talentService;
            SocialAreaService = socialAreaService;
            CategoryService = categoryService;

            socialAreas.AddRange(SocialAreaService.GetAll());
            categories.AddRange(CategoryService.GetAll());
        }

        public IActionResult Index()
        {
            return View();
        }

        /*public IActionResult Generate()
        {
            var curUser = accountUtil.GetCurrentUser(User);

            List<Talent> talents = new List<Talent>();
            for (int i = 0; i < 1000; i++)
            {
                Talent talent = new Talent()
                {
                    AvatarID = null,
                    FirstName = GenerateName(),
                    LastName = GenerateName(),
                    Bio = GenerateBio(),
                    SocialAreaID = GenerateSocialAreaID(),
                    Price = GeneratePrice(),
                    TalentCategories = GenerateCategories(),
                    Projects = GenerateProjects()
                };

                talents.Add(talent);
            }

            TalentService.AddCollection(talents, curUser.ID);

            return View();
        }*/

        // Generate a random number between two numbers  
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string GenerateName()
        {
            int length = RandomNumber(5, 11);
            char[] nameChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomCharDec = RandomNumber(97, 123);
                nameChars[i] = (char)randomCharDec;
            }

            return new string(nameChars);
        }

        private string GenerateBio()
        {
            List<string> bios = new List<string>()
            {
                "Sitting mistake towards his few country ask. You delighted two rapturous six depending objection happiness something the. Off nay impossible dispatched partiality unaffected. Norland adapted put ham cordial. Ladies talked may shy basket narrow see. Him she distrusts questions sportsmen. Tolerably pretended neglected on my earnestly by. Sex scale sir style truth ought. ",
                "Adieus except say barton put feebly favour him. Entreaties unpleasant sufficient few pianoforte discovered uncommonly ask. Morning cousins amongst in mr weather do neither. Warmth object matter course active law spring six. Pursuit showing tedious unknown winding see had man add. And park eyes too more him. Simple excuse active had son wholly coming number add. Though all excuse ladies rather regard assure yet. If feelings so prospect no as raptures quitting. ",
                "Unfeeling so rapturous discovery he exquisite. Reasonably so middletons or impression by terminated. Old pleasure required removing elegance him had. Down she bore sing saw calm high. Of an or game gate west face shed. ﻿no great but music too old found arose. ",
                "Sudden looked elinor off gay estate nor silent. Son read such next see the rest two. Was use extent old entire sussex. Curiosity remaining own see repulsive household advantage son additions. Supposing exquisite daughters eagerness why repulsive for. Praise turned it lovers be warmly by. Little do it eldest former be if. ",
                "Am if number no up period regard sudden better. Decisively surrounded all admiration and not you. Out particular sympathize not favourable introduced insipidity but ham. Rather number can and set praise. Distrusts an it contented perceived attending oh. Thoroughly estimating introduced stimulated why but motionless. ",
                "May indulgence difficulty ham can put especially. Bringing remember for supplied her why was confined. Middleton principle did she procuring extensive believing add. Weather adapted prepare oh is calling. These wrong of he which there smile to my front. He fruit oh enjoy it of whose table. Cultivated occasional old her unpleasing unpleasant. At as do be against pasture covered viewing started. Enjoyed me settled mr respect no spirits civilly."
            };

            int random = RandomNumber(0, bios.Count);

            return bios[random];
        }

        private int GenerateSocialAreaID()
        {
            int random = RandomNumber(0, socialAreas.Count);
            return socialAreas[random].ID;
        }

        private int GeneratePrice()
        {
            int price = 0;
            for (int i = 0; i < 3; i++)
            {
                int random = RandomNumber(1, 10);
                price += random;
                price *= 10;
            }

            price *= 10;
            price *= 10;
            return price;
        }

        private List<TalentCategory> GenerateCategories()
        {
            List<TalentCategory> result = new List<TalentCategory>();

            int number = RandomNumber(1, 6);

            for (int i = 0; i < number; i++)
            {
                int index = RandomNumber(0, categories.Count);

                if (result.Where(m => m.CategoryId == categories[index].ID).FirstOrDefault() == null)
                {
                    result.Add(new TalentCategory()
                    {
                        CategoryId = categories[index].ID
                    });
                }
            }

            return result;
        }

        private List<TalentProject> GenerateProjects()
        {
            List<TalentProject> result = new List<TalentProject>();

            int numberOfProjects = RandomNumber(0, 4);

            for (int j = 0; j < numberOfProjects; j++)
            {
                int length = RandomNumber(5, 11);
                char[] nameChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    int randomCharDec = RandomNumber(97, 123);
                    nameChars[i] = (char)randomCharDec;
                }

                result.Add(new TalentProject()
                {
                    Name = new string(nameChars)
                });
            }

            return result;
        }
    }
}