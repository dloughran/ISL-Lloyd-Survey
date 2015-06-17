using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Xml;
using LloydSurvey.Classes;

namespace LloydSurvey.Controllers
{
    public class SurveyController : Controller
    {
        //GET: Survey Welcome Page
        public ActionResult Index()
        {
            return View();
        }

        // GET: First Question Page
        public ActionResult One()
        {
            QuestionPageOneModel questionOne = null;
            if (Session["Q1Data"] != null)
            {
                questionOne = Session["Q1Data"] as QuestionPageOneModel;
            }
            else
            {
                Question qBase = XMLHelper.GetBaseQuestion(1);
                questionOne = new QuestionPageOneModel();
                if (qBase.Id != 0)
                {
                    questionOne.Id = qBase.Id;
                    questionOne.QuestionText = qBase.QuestionText;
                    questionOne.MaxSelections = 3;
                    questionOne.MinSelections = 3;
                    questionOne.Items = XMLHelper.GetQuestionCheckBoxes(1);
                    questionOne.OtherText = String.Empty;
                }
            }
            return View(questionOne);
        }

        // POST: First Question Page
        [HttpPost]
        public ActionResult One(QuestionPageOneModel questionOne)
        {
            Session["Q1Data"] = questionOne;
            return Redirect("/Survey/Map");
        }

        // GET Map
        public ActionResult Map()
        {
            MapModel mapModel = new MapModel();

            if(Session["MapData"] != null)
            {
                mapModel = Session["MapData"] as MapModel;
            }
            
            return View(mapModel);
        }

        // POST: Survey Map
        [HttpPost]
        public ActionResult Map(MapModel mapModel)
        {
            Session["MapData"] = mapModel;
            string destination = String.Format("/Survey/{0}", mapModel.RedirectTo);
            return Redirect(destination);
        }

        // GET: Second page of questions
        public ActionResult Two()
        {
            QuestionPageTwoModel p2m = null;
            if (Session["Q2Data"] != null)
            {
                p2m = Session["Q2Data"] as QuestionPageTwoModel;
            }
            else
            {
                p2m = new QuestionPageTwoModel();

                // Get Question 2 of Survey Set (First question on 2nd question page)
                Question qBase = XMLHelper.GetBaseQuestion(2);

                if (qBase.Id != 0)
                {
                    p2m.Q2QuestionText = qBase.QuestionText;
                    p2m.Q2TextEntry1 = String.Empty;
                    p2m.Q2TextEntry2 = String.Empty;
                    p2m.Q2TextEntry3 = String.Empty;
                }

                // Get Question 3 of Survey Set
                qBase = XMLHelper.GetBaseQuestion(3);

                if (qBase.Id != 0)
                {
                    p2m.Q3QuestionText = qBase.QuestionText;
                    p2m.Q3TextEntry1 = String.Empty;
                }

                // Get Question 4 of Survey Set
                qBase = XMLHelper.GetBaseQuestion(4);

                if (qBase.Id != 0)
                {
                    p2m.Q4QuestionText = qBase.QuestionText;
                    p2m.Q3TextEntry1 = String.Empty;
                }
            }
            return View(p2m);
        }

        // POST: Second page of questions
        [HttpPost]
        public ActionResult Two(QuestionPageTwoModel p2m)
        {
            Session["Q2Data"] = p2m;
            string destination = String.Format("/Survey/{0}", p2m.RedirectTo);
            return Redirect(destination);
            //return Redirect("/Survey/Thankyou");
        }

        //GET: Thank you (Last page of survey)
        public ActionResult Thankyou()
        {
            // ToDo Commit info from Session to database.
            QuestionPageOneModel q1 = Session["Q1Data"] as QuestionPageOneModel;
            MapModel mapData = Session["MapData"] as MapModel;
            QuestionPageTwoModel q2 = Session["Q2Data"] as QuestionPageTwoModel;
            if(q2 != null)
                q2.Q4TextEntry1 = String.IsNullOrEmpty(q2.Q4TextEntry1) ? String.Empty : q2.Q4TextEntry1.ToUpper();
            DBHelper dbhelper = new DBHelper();
            string dbError = dbhelper.writeSurveyResponse(q1, mapData, q2);
            ViewBag.Dberror = dbError;
            return View();
        }
    }
}