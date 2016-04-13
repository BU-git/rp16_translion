///<reference path="/Scripts/jquery-1.10.2.min.js"/>
/*NOT DONE YET*/
(function () {
    'use strict';

    class FindEmployee {
        constructor(employees) {
            this._employees = employees;   
        }

        findEmployee(name) {
            let foundEmployee;
            this._employees.each((indx, value) => {
                let employeeName = value.find('a.tlr-usr-info-label-btn > span').first().val;

            });

            return findEmployee || null;
        }

        //show concrete employee by index
        showEmployeeByIndx(indx) {
            hideEmployees(this._employees);
            this._employees.eq(indx).show();
        }

        //hide employees in jq object
        hideEmployees(employees) {
            employees = employees || $();
            if (employees.length > 0) {
                employees.hide();
                return true;
            }
            return false;
        }

        //show employees in jq object
        showEmployees(employees) {
            employees = employees || $();
            if (employees.length > 0) {
                employees.show();
                return true;
            }
            return false;
        }

        //add employee to jq object
        addEmployee(employee) {
            if (employee) {
                this._employees.add(employee);
                return true;
            }
            return false;
        }

        //add employee from server using ajax
        addRemoteEmployee(name) {
            let emplQuery = new Promise((resolve, reject) => {
                $.ajax({
                    method: 'GET',
                    url: '/Employer/GetEmployee/' + name,
                    withCredentials: true,
                    success: (data) => resolve($.parseJSON(data)),
                    error: (jqxhr, status, msg) => reject(msg)
                });
            });

            emplQuery.then(
                (data) => {
                    var jqEmployee = $(createEmployeeHtml(data));
                    _employees.add(jqEmployee);
                    showEmployeeByIndx(this._employees.length - 1);
                },
                (errMsg) => console.log(errMsg)
            );
        }

        //create employee html template
        createEmployeeHtml(employee) {
            let employeeHtml =
                `<div class="col-sm-3 col-md-3 col-lg-2 tlr-usr-info">
                    <a href="/Employer/EmployeeInfo/${employee.Id}" class="tlr-usr-inner-info">
                        <img src="/Content/Images/UserImg.png" alt="${employee.FullName}">
                    </a>
                    <a href="Employer/ChangeEmployeeName/${employee.Id}" class="tlr-usr-info-label-btn">
                        <span>${employee.FullName}</span>
                        <span class="glyphicon glyphicon-pencil"></span>
                    </a>
                </div>`;
            
            return employeeHtml;
        }
    };

    var findEmployees;

    $(document).ready(function () {
        let employees = $('');

    });
});