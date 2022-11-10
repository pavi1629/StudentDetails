using Microsoft.AspNetCore.Mvc;
using SampleCore.Core.Model;
using SampleCore.Core.IServices;

namespace StudentDetails.Controllers
{
    public class StudentInformation : Controller
    {
        private readonly IStudentDetailsServices _StudentdetailsServices;

        public StudentInformation(IStudentDetailsServices services)
        {
            _StudentdetailsServices = services;
        }
        #region CREATE A NEW INFORMATION STUDENT DETAILS
        [HttpPost]
        public IActionResult StudentEntry(StudentDetail value)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/api/StudentInformationApi/StudentEntry");
                var post = client.PostAsJsonAsync<StudentDetail>(client.BaseAddress, value);
                post.Wait();
                var checkresult = post.Result;
                return RedirectToAction("ReadList");
            }
        }
        [HttpGet]
        //To create View of this Action result
        public IActionResult Insert()
        {
            return View();
        }
        #endregion
        #region LIST OF STUDENT DETAILS   

        [HttpGet]
        public IActionResult ReadList()
        {
            using (HttpClient client = new HttpClient())
            {
                // We Cant Create New Instance 
                IList<StudentDetail> StudentsData = null;

                client.BaseAddress = new Uri("https://localhost:44366/api/StudentInformationApi/ReadList");
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<IList<StudentDetail>>();
                    readTask.Wait();

                    StudentsData = readTask.Result;
                }

                return View(StudentsData);

            }
        }
        #endregion
        #region EDIT THE STUDENT DETAILS   

        [HttpGet]
        public IActionResult Edit(int Student_id)
        {

            StudentDetail StudentsData = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/api/StudentInformationApi/Edit?Student_id=");
                //HTTP GET
                var responseTask = client.GetAsync(client.BaseAddress + Student_id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<StudentDetail>();
                    readTask.Wait();

                    StudentsData = readTask.Result;
                }
            }
            ViewBag.Edit = 6;
            return View("Insert", StudentsData);
        }

        #endregion
        #region DELETE THE STUDENT RECORDS
        [HttpGet]
        public IActionResult Delete(int Student_id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44366/api/StudentInformationApi/Delete?Student_id=");
                var deleteTask = client.DeleteAsync(client.BaseAddress + Student_id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ReadList");
                }
            }
            return RedirectToAction("ReadList");
        }
        #endregion
    }
}
