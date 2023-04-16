okrsapp.controller('employeesession', ['$scope', '$location', '$compile', 'localdata', 'dataservice', function ($scope, $location, $compile, localdata, dataservice) {

    $scope.addformData = {};
    $scope.previousgoalList = [];
    $scope.nextgoalList = [];
    $scope.searchGoalList = [];
    $scope.goalstatusList = [];
    $scope.addformData.newstatus = {};
    $scope.answers = [];
    $scope.survey = '';
    $scope.ErrorMessage = '';
    $scope.lastgoal = null;
    $scope.defaultcategory = 3;


    $scope.$watchCollection('answers', function (newVal, oldVal) {
        var el = $compile('<div class="row"><h4>Performance evaluation survey</h4></div><PerformanceSurveyDisplay selected-answers="answers" selected-survey="survey"></PerformanceSurveyDisplay>')($scope);
        $('#pdisplay').html(el);
    });

    $scope.Init = function () {

        sessionStorage.setItem('defaultgoalcategory', null);
        $scope.getuser();
        $scope.getgoalstatus();

        $scope.$watch('lastgoal', function (newValue, oldValue) {
            if (newValue != null && newValue != oldValue && $scope.containsAttendee(newValue.assignees, $scope.addformData.Attendee)) {
                $scope.nextgoalList.push(newValue);
                $scope.calculateSummary();

            }
        });


        $("#employeeSelect").on("change", function (e) {
            $scope.$apply(function () {

                if (e.val == '') {

                    $scope.addformData.Attendee = null;
                    $scope.previousgoalList = [];
                    $scope.nextgoalList = [];
                    $scope.searchGoalList = [];
                }
                else {
                    var newattendee = { "id": parseInt(e.val) };
                    $scope.addformData.Attendee = newattendee;
                    $scope.getpreviousgoals();
                    $scope.getnextgoals();
                    $scope.getSearchGoals();
                }
            });

        });

        $('#wizard-validation-form').on("submit", function (e) {

            $scope.$apply(function () {
                e.preventDefault();

                $scope.addformData.PerformanceSurveyId = $scope.survey.id;
                $scope.addformData.Goals = $scope.nextgoalList;
                $scope.addformData.Anwers = $scope.answers;

                var promise = dataservice.createEmpEvalSession($scope.addformData);

                promise
                      .success(function (data) {

                          $scope.loadingImage = false;
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Session created');
                          $location.path('/employeesessions');

                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             $scope.loadingImage = false;
                             $scope.ErrorMessage = $scope.parseErrors(data);
                         }
                         else
                             if (status == 404) {

                                 $scope.loadingImage = false;
                                 $scope.ErrorMessage = 'Requested link not found';
                             }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                     });
            });

        });
    }

    $scope.containsAttendee = function (attendees, attendee) {
        var i;

        for (i = 0; i < attendees.length; i++) {
            if (attendees[i].id == attendee.id) {
                return true;
            }
        }

        return false;
    }

    $scope.getuser = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchuser($scope.addformData);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              //console.log(data);
              $scope.userList = data;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
         });

    }

    $scope.calculateSummary = function () {
        $scope.previousCompleted = 0;
        $scope.previousTotal = 0;
        $scope.totalNew = 0;


        $scope.previousTotal = $scope.previousgoalList.length;

        angular.forEach($scope.previousgoalList, function (goal) {
            $scope.previousCompleted += goal.status.id == 3 ? 1 : 0;
        });

        angular.forEach($scope.nextgoalList, function (goal) {
            $scope.totalNew += goal.status.id == 1 ? 1 : 0;
        });

    }

    $scope.getpreviousgoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.empEvalToolPreviousGoals($scope.addformData.Attendee);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              //console.log(data);
              $scope.previousgoalList = data;
              $scope.calculateSummary();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else
                 if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
         });
    }

    $scope.getnextgoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.empEvalToolNextGoals($scope.addformData.Attendee);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.nextgoalList = data;
              $scope.calculateSummary();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else
                 if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
         });
    }

    $scope.getSearchGoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.empEvalToolSearchSessionGoal($scope.addformData.Attendee);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.searchGoalList = data;
              $scope.calculateSummary();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else
                 if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
         });
    }

    $scope.selgoal = {};
    $scope.selgoal.id = '';

    $scope.getcattemp = function (indx) {
        if ($scope.previousgoalList[indx].id == $scope.selgoal.id) return 'catedt';
        else return 'catdis';
    }

    $scope.editstatus = function (indx, goal) {
        $scope.selgoal = angular.copy(goal);

        for (var i = 0; i < $scope.goalstatusList.length; i++) {
            if ($scope.goalstatusList[i].id == $scope.previousgoalList[indx].status.id) {
                $scope.addformData.newstatus = $scope.goalstatusList[i];
            }
        }
    }

    $scope.updatestus = function (indx) {
        console.log($scope.addformData.newstatus);
        $scope.loadingImage = true;
        var promise = dataservice.editgoalstatus($scope.previousgoalList[indx].id, $scope.addformData.newstatus);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.previousgoalList[indx].status = $scope.addformData.newstatus;
              $scope.resetstus();
              $scope.getnextgoals();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = 'Requested link not found';

             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });

    }

    $scope.resetstus = function () {

        $scope.selgoal = {};
        $scope.selgoal.id = '';

    }


    $scope.getgoalstatus = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchgoalstatus();
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.goalstatusList = data;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = 'Requested link not found';
             }

             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });

    }



    $scope.addExistingGoal = function () {
        var found = false;
        var selectedgoal = JSON.parse($scope.addformData.existingGoal);

        for (var i = 0; i < $scope.nextgoalList.length; i++) {

            if ($scope.nextgoalList[i].id == selectedgoal.id) {
                found = true;
            }
        }

        if (!found) {
            $scope.nextgoalList.push(selectedgoal);
            $scope.calculateSummary();
        }

        existingObjSelect.val('').trigger("change");
    }

    $scope.parseErrors = function (response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
        return errors.toString();
    }

    $scope.Init();
}]);