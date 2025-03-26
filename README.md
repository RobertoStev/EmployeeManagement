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

<p>Once a leave request is created, it will be added to the employee's <b>My Requests</b> tab under the <b>Leave Requests</b> table with a Pending status, displaying all relevant details about the request.</p>

![Image](https://github.com/user-attachments/assets/ec1526e8-0a2a-43c1-83b8-23bb838367c1)

<p>In addition to standard leave requests, employees can also submit one or more sick leave requests in the <b>Sick Request</b> tab. To do so, they must:</p>
<ul>
  <li>Select a start date and end date (end date cannot be earlier than the start date).</li>
  <li>Provide a brief reason for the sick leave (e.g. "Flu recovery").</li>
  <li>Attach a medical report file.</li>
</ul>
<p>If any required field is left blank, the system will display an error message, and the request will not be submitted.</p>

![Image](https://github.com/user-attachments/assets/c8d7281e-2b0a-454c-9704-324fb8734581)

<p>Once a sick leave request is submitted, it will appear in the <b>My Requests</b> tab under the <b>Sick Leaves</b> table. The status of their request is Pending and there is View button where the employee can review his medical report attached for that sick request</p>

![Image](https://github.com/user-attachments/assets/4209010b-388b-490a-804c-18870234d702)

<h2><b>HR's View</b></h2>

<p>HR personnel have access to all standard employee features (including the <b>My Info</b> and <b>My Requests</b> tabs), along with additional administrative access to view all employees in the system and manage all leave and sick leave requests.</p>
<p>In addition to standard employee access, HR personnel can view all registered employees in the <b>All Employees Info</b> tab. This includes each employee’s profile picture, along with options to edit, delete, view details, and manage leave days for every employee in the system.</p>

![Image](https://github.com/user-attachments/assets/c8dfa113-52a9-4397-a913-6e3340a5fed4)

![Image](https://github.com/user-attachments/assets/a1e1a219-be86-49c0-818f-473254b121c1)

<p>HR personnel can: View employee details, Edit employee information, Delete employee and Manage leave days for employees</p>

<p>Edit employee information functionality</p>

![Image](https://github.com/user-attachments/assets/79770c7e-926f-49ac-877d-2b5eca4d6239)

<p>Delete employee functionality</p>

![Image](https://github.com/user-attachments/assets/7644e306-c002-420f-8162-9e21424469b5)

<p>View employee's details functionality</p>

![Image](https://github.com/user-attachments/assets/43e56af1-ad3c-4d10-b343-10045a89fe91)

<p>Manage days for employee functionality</p>

![Image](https://github.com/user-attachments/assets/14d6863b-895c-4fd8-b886-7552caab21ae)

<p>Example: HR has added 4 annual leave days and 5 bonus days for this employee.</p>

![Image](https://github.com/user-attachments/assets/009c103c-88ba-43b7-bd3b-9c8723adffb8)

<p>HR personnel can add new employees to the system in the <b>New Employee</b> tab by entering the following details: First name, Last name, Department, Job title, Email, A valid profile image</p>
<p>Once created, the new employee will appear in the <b>All Employees Info</b> tab alongside all other registered employees.</p>

![Image](https://github.com/user-attachments/assets/80f51891-1051-47fd-b2b9-4087f85faf1c)

![Image](https://github.com/user-attachments/assets/1e6c955a-11cc-4363-81a0-5f5ed45926b4)

<p>HR can view all leave requests in the <b>All Leave Requests</b> tab. HR can approve or decline a request. If approved, the system deducts the corresponding days (annual or bonus) from the employee’s balance based on the leave type used, the row highlights in light blue and the status for that request changes to Approved. If declined, no days are deducted, the row highlights in light red and the status for that request changes to Rejected.</p>

![Image](https://github.com/user-attachments/assets/83104fae-082a-4258-b585-8d5867c5fc93)

![Image](https://github.com/user-attachments/assets/1a03f57d-69ae-46b3-b858-3d25d785fa61)

<p>HR can view all sick requests created by employees in the <b>All Sick Requests</b> tab. HR can approve or decline a sick request. If approved, the row is colored light blue and the status changes to Approved. If declined, the row is colored light red and the status changes to Rejected.</p>
<p>Employees can log in to their accounts to view the statuses of their leave requests and sick requests.</p>

![Image](https://github.com/user-attachments/assets/a4878808-bcea-41fa-a632-ad2f5487b68a)

![Image](https://github.com/user-attachments/assets/7cc2e3bb-826b-40f2-9c4f-82940a6ff5db)

<p>They employees can log in into their account and see the status of their requests.</p>

![Image](https://github.com/user-attachments/assets/f383cd91-a043-4a23-87cd-da7edc45a1f8)

![Image](https://github.com/user-attachments/assets/9bbce640-b3a6-42ad-8715-7eda49204b7e)

<p>HR can delete employees from the system. When an employee is deleted, all their approved leave and sick requests remain in the system, while any pending or declined requests are automatically removed. For example, if we delete employee Sophia Bennett, we can check both the leave requests and sick requests tables to verify that only her approved requests are preserved.</p>

![Image](https://github.com/user-attachments/assets/8d6c50bb-5d74-4b14-832c-6e259a4e48bf)

![Image](https://github.com/user-attachments/assets/f0b2919e-a23d-45aa-83d6-cc56e1f91d89)
