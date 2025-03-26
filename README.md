<h1><b>Employee Management System</b></h1>
<p>An ASP.NET MVC application for managing employee leave and sick leave requests with role-based access control.</p>

<h2><b>Overview</b></h2>
<p>This system is built with ASP.NET MVC and SQL Server, using Individual Accounts for authentication and role management. There are two user roles:</p>
<ul>
  <li><b>Employee:</b> Can submit leave and sick leave requests.</li>
  <li><b>HR:</b> Has full <b>CRUD (Create, Read, Update, Delete)</b> control over employees and can manage leave balances.</li>
</ul>

<p>Both Employees and HR users can create leave and sick leave requests, while HR employees can adjust annual and bonus leave days for employees as needed.</p>

<p>This is the landing page where unregistered or non-logged-in users are redirected when the application starts. It provides a brief overview of the system and its functionality.</p>

![Image](https://github.com/user-attachments/assets/5bb0340d-dd3f-4050-8248-3663e2ee2869)

<h2><b>Employee's View</b></h2>
<p>Now, let's consider a scenario where a new user wants to register in the system. Clicking the Register button opens a registration form. After entering a valid email and password, the user successfully registers but must wait for an HR employee to add them to the system.</p>

![Image](https://github.com/user-attachments/assets/3f1cb13d-02d9-43d3-8a35-48d2f1746a64)

<p>The <b>Employee Management</b> tab serves as the main dashboard of the application, providing an overview and key functionalities for managing employees.</p>

![Image](https://github.com/user-attachments/assets/201b4ad8-2e6e-46e9-978d-c12157441fae)

<p>Once added, the user can log in and access their account. In the <b>My Info</b> tab, users can view their personal details, including their full name, department, job title, email, remaining annual and bonus leave days, and their expiration dates. Every new employee starts with 21 annual leave days by default.</p>

![Image](https://github.com/user-attachments/assets/321db116-08a2-41e2-bb0a-c7a261696cf7)

<p>In the <b>My Requests</b> tab, logged-in employees can view their leave requests and sick leave records. If no requests have been made yet, the page will indicate that there are none.</p>

![Image](https://github.com/user-attachments/assets/a19ba336-020d-4a06-ad23-968fad12e3ce)

<p>In the <b>Leave Request</b> tab, employees can submit leave requests. Each employee can have multiple leave requests, forming a one-to-many relationship. To submit a request, the employee must:
</p>
<ul>
  <li>Enter a start date and end date (the end date cannot be earlier than the start date).</li>
  <li>Provide a comment.</li>
  <li>Choose whether to use Annual or Bonus leave days.</li>
</ul>
<p>If the employee lacks sufficient leave days or leaves any required fields blank, an error message will appear, and the request will not be processed.
</p>

![Image](https://github.com/user-attachments/assets/aca215b2-b9ae-4fe1-ac2b-2dfc955cffeb)

<p>Once a leave request is created, it will be added to the employee's <b>My Requests</b> tab under the <b>Leave Requests</b> table with a Pending status, displaying all relevant details about the request.</p>

![Image](https://github.com/user-attachments/assets/2df72e74-f6de-4f39-b6c2-22a7b69548d6)

<p>In addition to standard leave requests, employees can also submit one or more sick leave requests in the <b>Sick Request</b> tab. To do so, they must:</p>
<ul>
  <li>Select a start date and end date (end date cannot be earlier than the start date).</li>
  <li>Provide a brief reason for the sick leave (e.g. "Flu recovery").</li>
  <li>Attach a medical report file.</li>
</ul>
<p>If any required field is left blank, the system will display an error message, and the request will not be submitted.</p>

![Image](https://github.com/user-attachments/assets/f0328217-c0e0-4755-85c4-ff9de7af01b0)

<p>Once a sick leave request is submitted, it will appear in the <b>My Requests</b> tab under the <b>Sick Leaves</b> table. The status of their request is Pending and there is View button where the employee can review his medical report attached for that sick request.</p>

![Image](https://github.com/user-attachments/assets/c14748f4-f11a-48dc-adc0-339addffad9d)

<h2><b>HR's View</b></h2>

<p>HR personnel have access to all standard employee features (including the <b>My Info</b> and <b>My Requests</b> tabs), along with additional administrative access to view all employees in the system and manage all leave and sick leave requests.</p>
<p>In addition to standard employee access, HR personnel can view all registered employees in the <b>All Employees Info</b> tab. This includes each employee’s profile picture, along with options to edit, delete, view details, and manage leave days for every employee in the system.</p>

![Image](https://github.com/user-attachments/assets/df0d93fb-5e99-4072-83de-db0275639536)

![Image](https://github.com/user-attachments/assets/3c886d7a-3689-4e9e-8cfd-87e829ca58c3)

<p>HR personnel can: View employee details, Edit employee information, Delete employee and Manage leave days for employees.</p>

<p>Edit employee information functionality.</p>

![Image](https://github.com/user-attachments/assets/7183c379-1f48-434e-ab75-740da648b978)

<p>Delete employee functionality.</p>

![Image](https://github.com/user-attachments/assets/cbbf2703-2516-45d4-8288-d5dfb386e990)

<p>View employee's details functionality.</p>

![Image](https://github.com/user-attachments/assets/125e162e-efd3-4c7a-baca-85cbdaf3ecf6)

<p>Manage days for employee functionality.</p>

![Image](https://github.com/user-attachments/assets/aaf3b0df-ec21-4539-9800-c4edd3d33386)

<p>Example: HR has added 4 annual leave days and 5 bonus days for this employee.</p>

![Image](https://github.com/user-attachments/assets/d19b7606-2311-4441-b8d9-0b0976d2142e)

<p>HR personnel can add new employees to the system in the <b>New Employee</b> tab by entering the following details: First name, Last name, Department, Job title, Email, A valid profile image.</p>
<p>Once created, the new employee will appear in the <b>All Employees Info</b> tab alongside all other registered employees.</p>

![Image](https://github.com/user-attachments/assets/41bb546e-540c-43f3-8f8a-bb70fb010eed)

![Image](https://github.com/user-attachments/assets/5e56333a-ec54-49e4-88ac-061460b4295e)

<p>HR can view all leave requests in the <b>All Leave Requests</b> tab. HR can approve or decline a request. If approved, the system deducts the corresponding days (annual or bonus) from the employee’s balance based on the leave type used, the row highlights in light blue and the status for that request changes to Approved. If declined, no days are deducted, the row highlights in light red and the status for that request changes to Rejected.</p>

![Image](https://github.com/user-attachments/assets/ec73fc77-75e4-44a2-b458-25434fac5f27)

![Image](https://github.com/user-attachments/assets/de2da4ae-b4e5-49ae-a2d0-6940e38a56c7)

<p>HR can view all sick requests created by employees in the <b>All Sick Requests</b> tab. HR can approve or decline a sick request. If approved, the row is colored light blue and the status changes to Approved. If declined, the row is colored light red and the status changes to Rejected.</p>

![Image](https://github.com/user-attachments/assets/6dacf425-2506-447e-8848-7641ccff822a)

![Image](https://github.com/user-attachments/assets/cde72e79-05c5-4ecc-a368-db3d2b111f92)

<p>Employees can log in to their accounts to view the statuses of their leave requests and sick requests.</p>

![Image](https://github.com/user-attachments/assets/48c2b5e7-7d46-49af-9330-7c53555c4f7d)

![Image](https://github.com/user-attachments/assets/e4a261b0-5f88-4faa-9805-307563af83c5)

<p>HR can delete employees from the system. When an employee is deleted, all their approved leave and sick requests remain in the system, while any pending or declined requests are automatically removed. For example, if we delete employee Sophia Bennett, we can check both the leave requests and sick requests tables to verify that only her approved requests are preserved.</p>

![Image](https://github.com/user-attachments/assets/f7afd8aa-592d-4a11-8802-9c7fe95680f1)

![Image](https://github.com/user-attachments/assets/f9f03fbd-637f-4e52-a7be-76f6dc7ec381)
