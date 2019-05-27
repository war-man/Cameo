using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cameo.Data;
using Cameo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Cameo.Services.Interfaces;

namespace Cameo.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService PostService;

        public PostsController(IPostService postService)
        {
            PostService = postService;
        }

        public IActionResult Index()
        {
            var posts = PostService.GetAll();
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //return NotFound();

            //return BadRequest();

            try
            {
                int j = 0;
                int k = 10 / j;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            

            //create
            //var newPost = new Post()
            //{
            //    Title = "adasdf",
            //    AuthorID = id
            //};
            //PostService.Add(newPost, id);

            //List<Post> newPosts = new List<Post>()
            //{
            //    new Post()
            //    {
            //        Title = "A1",
            //        AuthorID = id
            //    },
            //    new Post()
            //    {
            //        Title = "A2",
            //        AuthorID = id
            //    },
            //};
            //PostService.AddCollection(newPosts, id);


            //update
            //var oldPost = PostService.GetByID(6);
            //oldPost.Title = "B1";
            //PostService.Update(oldPost, id);

            //var oldPost7 = PostService.GetByID(7);
            //oldPost.Title = "B2";
            //var oldPost8 = PostService.GetByID(8);
            //oldPost.Title = "B3";

            //List<Post> oldPosts = new List<Post>()
            //{
            //    oldPost7,
            //    oldPost8
            //};
            //PostService.UpdateCollection(oldPosts, id);


            //delete
            //var oldPost = PostService.GetByID(6);
            //PostService.Delete(oldPost, id);

            //var oldPost7 = PostService.GetByID(7);
            //var oldPost8 = PostService.GetByID(8);

            //List<Post> oldPosts = new List<Post>()
            //{
            //    oldPost7,
            //    oldPost8
            //};
            //PostService.DeleteCollection(oldPosts, id);


            //deletePermanently
            //var oldPost = PostService.GetByID(6);
            //PostService.DeletePermanently(oldPost, id);

            //var oldPost7 = PostService.GetByID(7);
            //var oldPost8 = PostService.GetByID(8);

            //List<Post> oldPosts = new List<Post>()
            //{
            //    oldPost7,
            //    oldPost8
            //};
            //PostService.DeletePermanentlyCollection(oldPosts, id);


            return View(posts);
            //return View(await _context.Posts.ToListAsync());
        }



        //private readonly ApplicationDbContext _context;

        //public PostsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //// GET: Posts
        //public async Task<IActionResult> Index()
        //{
        //    var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    //ApplicationUser user = _context.Users.Include(m => m.PostsCreated)
        //    //    .First(m => m.Id == "2e6c91ff-41ed-4d7d-87df-a942293efd66");
        //    //var posts = user.PostsCreated.ToList();

        //    ApplicationUser user2 = _context.Users.Find("2e6c91ff-41ed-4d7d-87df-a942293efd66");
        //    _context.Entry(user2).Collection(m => m.PostsCreated).Load();
        //    var posts2 = user2.PostsCreated.ToList();


        //    return View(await _context.Posts.ToListAsync());
        //}

        //// GET: Posts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        //// GET: Posts/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Posts/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Title")] Post post)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(post);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(post);
        //}

        //// GET: Posts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts.FindAsync(id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(post);
        //}

        //// POST: Posts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Title")] Post post)
        //{
        //    if (id != post.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(post);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PostExists(post.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(post);
        //}

        //// GET: Posts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        //// POST: Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var post = await _context.Posts.FindAsync(id);
        //    _context.Posts.Remove(post);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PostExists(int id)
        //{
        //    return _context.Posts.Any(e => e.ID == id);
        //}
    }
}
