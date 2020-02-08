using Cicada.ViewModels.Document;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Cicada.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Article(string id)
        {
            ArticleDto article = new ArticleDto();

            string documentContext = "<h4>Article Not Found!</h4>";
            //try get the md files by id.
            string basePath = AppContext.BaseDirectory;
            string path = Path.Combine(basePath, "Docs", id + ".md");

            if (System.IO.File.Exists(path))
            {
                string mdFileStr = System.IO.File.ReadAllText(path);

                // Configure the pipeline with all advanced extensions active
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

                //change to html string
                documentContext = Markdown.ToHtml(mdFileStr, pipeline); 
            }

            article.Html = documentContext;

            //display it.
            return View(article);
        }
    }
}