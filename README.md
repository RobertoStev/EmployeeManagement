<h1><b>Employee Management System</b></h1>
<p>An ASP.NET MVC application for managing employee leave and sick leave requests with role-based access control.</p>

<h2><b>Overview</b></h2>
<p>This system is built with ASP.NET MVC and SQL Server, using Individual Accounts for authentication and role management. There are two user roles:</p>
<ul>
  <li><b>Employee:</b> Can submit leave and sick leave requests.</li>
  <li><b>HR:</b> Has full <b>CRUD (Create, Read, Update, Delete)</b> control over employees and can manage leave balances.</li>
</ul>

<p>Both Employees and HR users can request leave, while HR employees can adjust annual and bonus leave days for employees as needed.</p>

<p>This is the landing page where unregistered or non-logged-in users are redirected when the application starts. It provides a brief overview of the system and its functionality.</p>

![Image](https://github.com/user-attachments/assets/a7dcf484-eb81-44ef-b508-7ba65f3cf67e)

<h2><b>Employee's View</b></h2>
<p>Now, let's consider a scenario where a new user wants to register in the system. Clicking the Register button opens a registration form. After entering a valid email and password, the user successfully registers but must wait for an HR employee to add them to the system.</p>

![Image](https://github.com/user-attachments/assets/703d517f-4998-4b64-979f-5bef349fc801)

<p>The <b>Employee Management</b> tab serves as the main dashboard of the application, providing an overview and key functionalities for managing employees.</p>

![Image](https://github.com/user-attachments/assets/a250e387-dc21-485d-8bb7-2fcc8fd2deb5)

<p>Once added, the user can log in and access their account. In the <b>My Info</b> tab, users can view their personal details, including their full name, department, job title, email, remaining annual and bonus leave days, and their expiration dates. Every new employee starts with 21 annual leave days by default.</p>

![Image](https://github.com/user-attachments/assets/cedadcd2-14bb-4c46-abb7-6650022e3c22)

<p>In the <b>My Requests</b> tab, logged-in employees can view their leave requests and sick leave records. If no requests have been made yet, the page will indicate that there are none.</p>

![Image](https://github.com/user-attachments/assets/0c218bb7-2f0b-4dfb-ab90-aa74331e9864)

<p>In the <b>Leave Request</b> tab, employees can submit leave requests. Each employee can have multiple leave requests, forming a one-to-many relationship. To submit a request, the employee must:
</p>
<ul>
  <li>Enter a start date and end date (the end date cannot be earlier than the start date).</li>
  <li>Provide a comment.</li>
  <li>Choose whether to use Annual or Bonus leave days.</li>
</ul>
<p>If the employee lacks sufficient leave days or leaves any required fields blank, an error message will appear, and the request will not be processed.
</p>

![Image](https://github.com/user-attachments/assets/da835275-9a83-4ec8-a5e0-34103f4a81f9)
