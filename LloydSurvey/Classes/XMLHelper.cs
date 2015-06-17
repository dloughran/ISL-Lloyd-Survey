using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace LloydSurvey.Classes
{
    public static class XMLHelper
    {
        public static Question GetBaseQuestion(int id)
        {
            XElement element = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Questions.xml"));
            var baseQuestion = from q in element.Elements("question")
                           where (int)q.Element("questionnumber") == id
                           select q;
            Question question = new Question();
            foreach(XElement x in baseQuestion)
            {
                question.Id = (int)x.Element("questionnumber");
                question.QuestionText = (string)x.Element("questiontext");
                if(x.Element("numberofentries") != null)
                {
                    question.NumberOfEntries = (int)x.Element("numberofentries");
                }
            }

            return question;
        }

        public static List<OptionCheckBox> GetQuestionCheckBoxes(int questionId)
        {
            XElement element = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Questions.xml"));
            var baseQuestion = from q in element.Elements("question")
                          where (int)q.Element("questionnumber") == questionId
                          select q;
            List<OptionCheckBox> items = new List<OptionCheckBox>();
            var i = 1;
            foreach(XElement x in baseQuestion.Elements("questionitems").Elements("item"))
            {
                var label = (string)x.Value;
                items.Add(new OptionCheckBox
                {
                    Id = i,
                    QuestionId = questionId,
                    Type = "CheckBox",
                    Label = label,
                    Selected = false
                });
                i += 1;
            }
            return items;


        }

        public static XmlDocument GetSurveyXml(QuestionPageOneModel p1Model, MapModel mapModel, QuestionPageTwoModel p2Model)
        {
            //Create question response and map marker XML
            XmlDocument doc = new XmlDocument();
            try
            {
                // Document root node
                XmlNode submissionNode = doc.CreateElement("submission");
                doc.AppendChild(submissionNode);
                // SectionOne (Page One Question) nodes
                XmlNode sectionOneNode = doc.CreateElement("sectionone");
                submissionNode.AppendChild(sectionOneNode);
                XmlNode questionsNode1 = doc.CreateElement("questions");
                sectionOneNode.AppendChild(questionsNode1);
                XmlNode questionNode1 = doc.CreateElement("question");
                questionsNode1.AppendChild(questionNode1);
                XmlNode promptNode1 = doc.CreateElement("prompt");
                promptNode1.InnerText = p1Model.QuestionText;
                questionNode1.AppendChild(promptNode1);
                XmlNode responsesNode1 = doc.CreateElement("responses");
                questionNode1.AppendChild(responsesNode1);
                for (int i = 0; i < p1Model.Items.Count; i++)
                {
                    if (p1Model.Items[i].Selected)
                    {
                        XmlNode responseNode = doc.CreateElement("response");
                        if (p1Model.Items[i].Label.Equals("Other"))
                        {
                            responseNode.InnerText = p1Model.OtherText;
                        }
                        else
                        {
                            responseNode.InnerText = p1Model.Items[i].Label;
                        }
                        responsesNode1.AppendChild(responseNode);
                    }
                }
                //Map Data Section
                XmlNode mapdataNode = doc.CreateElement("mapdata");
                submissionNode.AppendChild(mapdataNode);
                XmlNode markersNode = doc.CreateElement("markers");
                mapdataNode.AppendChild(markersNode);
                string mkrs = mapModel.Markers[0];
                var js = new JavaScriptSerializer();
                List<SlimMarker> markerList = new List<SlimMarker>();
                markerList = js.Deserialize<List<SlimMarker>>(mkrs);
                for (int i = 0; i < markerList.Count; i++)
                {
                    XmlNode markerNode = doc.CreateElement("marker");
                    XmlNode latNode = doc.CreateElement("lat");
                    latNode.InnerText = markerList[i].Lat;
                    XmlNode lngNode = doc.CreateElement("lng");
                    lngNode.InnerText = markerList[i].Lng;
                    XmlNode typeNode = doc.CreateElement("type");
                    typeNode.InnerText = markerList[i].Type;
                    XmlNode userCommentNode = doc.CreateElement("usercomment");
                    userCommentNode.InnerText = markerList[i].Comment;
                    markerNode.AppendChild(latNode);
                    markerNode.AppendChild(lngNode);
                    markerNode.AppendChild(typeNode);
                    markerNode.AppendChild(userCommentNode);
                    markersNode.AppendChild(markerNode);
                }
                //SectionTwo (Last page of survey questions)
                XmlNode sectionTwoNode = doc.CreateElement("sectiontwo");
                submissionNode.AppendChild(sectionTwoNode);
                XmlNode questionsNode2 = doc.CreateElement("questions");
                sectionTwoNode.AppendChild(questionsNode2);
                XmlNode question2 = doc.CreateElement("question");
                questionsNode2.AppendChild(question2);
                XmlNode promptNode2 = doc.CreateElement("prompt");
                promptNode2.InnerText = p2Model.Q2QuestionText;
                question2.AppendChild(promptNode2);
                XmlNode responsesNode2 = doc.CreateElement("responses");
                question2.AppendChild(responsesNode2);
                XmlNode responseNode21 = doc.CreateElement("response");
                responseNode21.InnerText = p2Model.Q2TextEntry1;
                XmlNode responseNode22 = doc.CreateElement("response");
                responseNode22.InnerText = p2Model.Q2TextEntry2;
                XmlNode responseNode23 = doc.CreateElement("response");
                responseNode23.InnerText = p2Model.Q2TextEntry3;
                responsesNode2.AppendChild(responseNode21);
                responsesNode2.AppendChild(responseNode22);
                responsesNode2.AppendChild(responseNode23);

                //Final Comments text area
                XmlNode question3 = doc.CreateElement("question");
                questionsNode2.AppendChild(question3);
                XmlNode prompt3 = doc.CreateElement("prompt");
                prompt3.InnerText = p2Model.Q3QuestionText;
                question3.AppendChild(prompt3);
                XmlNode responses3 = doc.CreateElement("responses");
                question3.AppendChild(responses3);
                XmlNode response3 = doc.CreateElement("response");
                response3.InnerText = p2Model.Q3TextEntry1;
                responses3.AppendChild(response3);

                //Postal Code
                XmlNode question4 = doc.CreateElement("question");
                questionsNode2.AppendChild(question4);
                XmlNode prompt4 = doc.CreateElement("prompt");
                prompt4.InnerText = p2Model.Q4QuestionText;
                question4.AppendChild(prompt4);
                XmlNode responses4 = doc.CreateElement("responses");
                question4.AppendChild(responses4);
                XmlNode response4 = doc.CreateElement("response");
                response4.InnerText = p2Model.Q4TextEntry1;
                responses4.AppendChild(response4);
            }
            catch (Exception ex) 
            {
                //TODO:Log Exception
            }
            return doc;
        }
    }
}