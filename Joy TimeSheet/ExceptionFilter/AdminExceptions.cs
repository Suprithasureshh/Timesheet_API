using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TimeSheet.ExceptionFilter
{
    //Client
    public class ClientIdException : Exception
    {
        public ClientIdException() { }
        public ClientIdException(string message) : base(message) { }
    }

    //Project
    public class ProjectIdNotExistException : Exception
    {
        public ProjectIdNotExistException() { }
        public ProjectIdNotExistException(string message) : base(message) { }
    }
    public class ProjectNameExistException : Exception
    {
        public ProjectNameExistException() { }
        public ProjectNameExistException(string message) : base(message) { }
    }
    public class ProjectCodeExistException : Exception
    {
        public ProjectCodeExistException() { }
        public ProjectCodeExistException(string message) : base(message) { }
    }
    public class ClientNotExistException : Exception
    {
        public ClientNotExistException() { }
        public ClientNotExistException(string message) : base(message) { }
    }

    //Designation
    public class DesignationIdException : Exception
    {
        public DesignationIdException() { }
        public DesignationIdException(string message) : base(message) { }
    }

    public class DesignationNameException : Exception
    {
        public DesignationNameException() { }
        public DesignationNameException(string message) : base(message) { }
    }

    //EmployeeType
    public class EmployeeTypeIdException : Exception
    {
        public EmployeeTypeIdException() { }
        public EmployeeTypeIdException(string message) : base(message) { }
    }
    public class EmployeeTypeNameException : Exception
    {
        public EmployeeTypeNameException() { }
        public EmployeeTypeNameException(string message) : base(message) { }
    }

    //Employee
    public class EmployeeIdNotExistException : Exception
    {
        public EmployeeIdNotExistException() { }
        public EmployeeIdNotExistException(string message) : base(message) { }
    }
    public class EmployeeEmailExistException : Exception
    {
        public EmployeeEmailExistException() { }
        public EmployeeEmailExistException(string message) : base(message) { }
    }
    public class EmployeeContactExistException : Exception
    {
        public EmployeeContactExistException() { }
        public EmployeeContactExistException(string message) : base(message) { }
    }

    //EmployeeProject

    //HrContactInfo
    public class HrIdException : Exception
    {
        public HrIdException() { }
        public HrIdException(string message) : base(message) { }
    }
    public class HrMailExistException : Exception
    {
        public HrMailExistException() { }
        public HrMailExistException(string message) : base(message) { }
    }
    public class HrMailNotExistException : Exception
    {
        public HrMailNotExistException() { }
        public HrMailNotExistException(string message) : base(message) { }
    }
    public class HrConatactException : Exception
    {
        public HrConatactException() { }
        public HrConatactException(string message) : base(message) { }
    }




    public class CustomExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                //Client
                case ClientIdException:
                    context.Result = new BadRequestObjectResult("No client has the entered id");
                    break;

                //Project
                case ProjectIdNotExistException:
                    context.Result = new BadRequestObjectResult("No project with the entered id");
                    break;
                case ProjectNameExistException:
                    context.Result = new BadRequestObjectResult("Project Name already exists");
                    break;
                case ProjectCodeExistException:
                    context.Result = new BadRequestObjectResult("Project Code already exists");
                    break;
                case ClientNotExistException:
                    context.Result = new BadRequestObjectResult("No Client with the entered id");
                    break;

                //Designation
                case DesignationIdException:
                    context.Result = new BadRequestObjectResult("No designation has the entered id");
                    break;
                case DesignationNameException:
                    context.Result = new BadRequestObjectResult("Designation already exists");
                    break;

                //EmployeeType
                case EmployeeTypeIdException:
                    context.Result = new BadRequestObjectResult("No employee type has the entered id");
                    break;
                case EmployeeTypeNameException:
                    context.Result = new BadRequestObjectResult("Employee Type already exists");
                    break;

                //Employee
                case EmployeeIdNotExistException:
                    context.Result = new BadRequestObjectResult("No Employee with the entered id");
                    break;
                case EmployeeEmailExistException:
                    context.Result = new BadRequestObjectResult("Official mail already exists");
                    break;
                case EmployeeContactExistException:
                    context.Result = new BadRequestObjectResult("Contact Number already exists");
                    break;

                //HrContactInfo
                case HrIdException:
                    context.Result = new BadRequestObjectResult("No HR has the entered id");
                    break;

                case HrMailExistException:
                    context.Result = new BadRequestObjectResult("Email already exists");
                    break;

                case HrMailNotExistException:
                    context.Result = new BadRequestObjectResult("Email does not exists");
                    break;

                case HrConatactException:
                    context.Result = new BadRequestObjectResult("Contact already exists");
                    break;




                case ArgumentNullException:
                    context.Result = new BadRequestObjectResult("Argument Null Exception exception occurred.");
                    break;
            }
            context.ExceptionHandled = true;
        }
    }
}
