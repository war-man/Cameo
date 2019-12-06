using Cameo.Models;
using Cameo.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cameo.ViewModels
{
    public class VideoDetailsVM
    {
        public int RequestID { get; set; }
        public AttachmentDetailsVM Video { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public TalentShortInfoForVideoPageVM Talent { get; set; }

        //TO-DO:
        //public bool Reviewable { get; set; } = false;

        public VideoDetailsVM() { }

        public VideoDetailsVM(VideoRequest model)
        {
            if (model == null)
                return;

            RequestID = model.ID;
            Video = new AttachmentDetailsVM(model.Video);
            To = model.To;
            From = model.From;
            Talent = new TalentShortInfoForVideoPageVM(model.Talent);            
        }
    }
}
