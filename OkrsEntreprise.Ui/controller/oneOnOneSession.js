okrsapp.controller('oneOnOneSession', ['$scope', '$location', '$routeParams', 'localdata', 'dataservice', '$rootScope', function ($scope, $location, $routeParams, localdata, dataservice, $rootScope) {
    $rootScope.sessionUser = {};
    $scope.addformData = {};
    $scope.previousgoalList = [];
    $scope.nextgoalList = [];
    $scope.searchGoalList = [];
    $scope.goalstatusList = [];
    $scope.addformData.newstatus = {};
    $scope.ErrorMessage = '';
    $scope.lastgoal = null;
    $scope.defaultcategory = 3;
    $scope.isopen = true;
    $scope.currentUser = {};

    $scope.Init = function () {
        //$(".content").niceScroll({
        //    cursorcolor: "#cfcfcf",
        //    cursorwidth: "8"
        //});
        //$(".scrollerPanel").niceScroll({
        //    cursorcolor: "#cfcfcf",
        //    cursorwidth: "8"
        //});
        
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
                    var newattendee = { "id": parseInt(angular.fromJson(e.val).id) };
                    $scope.addformData.Attendee = newattendee;

                    $scope.currentUser = angular.fromJson(e.val);
                    $rootScope.sessionUser = $scope.currentUser;
                    console.log($scope.currentUser);
                    $scope.getpreviousgoals();
                    $scope.getnextgoals();
                    $scope.getSearchGoals();
                }
            });

        });

        $('#wizard-validation-form').on("submit", function (e) {

            $scope.$apply(function () {
                e.preventDefault();

                $scope.addformData.Goals = $scope.nextgoalList;
                var promise = dataservice.createsession($scope.addformData);

                promise
                      .success(function (data) {

                          $scope.loadingImage = false;
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Session created');
                          $location.path('/sessions');

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
                         $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
                     });
            });

        });
    }

    $scope.containsAttendee = function (attendees,attendee) {
        var i;

        for (i = 0; i < attendees.length; i++) {
            if (attendees[i].id == attendee.id) {
                return true;
            }
        }

        return false;
    }

    $scope.goalAdded = function (addedGoal) {
        $scope.nextgoalList.push(addedGoal);
        $scope.calculateSummary();
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
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
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
        var promise = dataservice.previousgoals($scope.addformData.Attendee);
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
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });
    }

    $scope.getnextgoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.nextgoals($scope.addformData.Attendee);
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
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });
    }

    $scope.getSearchGoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchgoal();
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
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
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
        console.log(selectedgoal);
        if (!selectedgoal)
            $.Notification.autoHideNotify('error', 'top right', 'Success', "goal not selected!");
        for (var i = 0; i < $scope.nextgoalList.length; i++) {

            if ($scope.nextgoalList[i].id == selectedgoal.id) {
                found = true;
            }
        }

        if (!found) {
            $scope.nextgoalList.push(selectedgoal);
            $scope.calculateSummary();
        }
        console.log(found);
        if (found)
            $.Notification.autoHideNotify('error', 'top right', 'Success',"goal alreary exists!" );
        else
            $.Notification.autoHideNotify('success', 'top right', 'Success', selectedgoal.title + ' added to list');

        $scope.addformData.existingGoal = {};
        //$("#existingObjSelect").val('').trigger("change");
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
    $scope.editisopen = function (indx) {
        swal({
            title: "Are you sure?",
            text: "You can change the status if you need in future.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, " + ($scope.previousgoalList[indx].isopen ? "close" : "open") + " it!",
            closeOnConfirm: true
        }, function () {
            var promise = null;
            promise = dataservice.editgoalisopen($scope.previousgoalList[indx].id, !$scope.previousgoalList[indx].isopen);

            promise.success(function (data) {
                $scope.previousgoalList[indx].isopen = !$scope.previousgoalList[indx].isopen;
                if (!$scope.previousgoalList[indx].isopen)
                    $.Notification.autoHideNotify('success', 'top right', 'Success', 'Objective closed!');
                else
                    $.Notification.autoHideNotify('success', 'top right', 'Success', 'Objective opened!');
            })
                .error(function (data, status, headers, config) {
                    if (status == 400) {
                        $scope.loadingImage = false;
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    } else if (status == 404) {

                        $scope.loadingImage = false;
                        $scope.ErrorMessage = 'Requested link not found';

                    }
                    $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                });
        })
    };
    $scope.Init();
}]);