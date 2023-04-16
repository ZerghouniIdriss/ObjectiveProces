okrsapp.controller('goal', ['$scope', '$location', '$timeout', '$routeParams', 'localdata', 'dataservice', 'dashboardservice', function ($scope, $location, $timeout, $routeParams, localdata, dataservice, dashboardservice) {
 
    $scope.ErrorMessage = '';
    $scope.searchformData = {};
    $scope.goalList = [];
    $scope.lastgoal = null;
    $scope.defcat = 1;
    $scope.goalstatusList = [];
    //$scope.OnTrackObjectives = [];
    //$scope.AtRiskObjectives = [];
    //$scope.DelayedObjectives = [];
    $scope.editGoalId = 24;
    $scope.isCollapsedTrack = false;
    $scope.isCollapsedRisk = true;
    $scope.isCollapsedDelayed = true;
    $scope.CollapsePlane = function (p) {
        if (p == 1) {
            $scope.isCollapsedTrack = !$scope.isCollapsedTrack;
            $scope.isCollapsedRisk = true;
            $scope.isCollapsedDelayed = true;
        }
        else if (p == 2) {
            $scope.isCollapsedRisk = !$scope.isCollapsedRisk;
            $scope.isCollapsedTrack = true;
            $scope.isCollapsedDelayed = true;
        }
        else if (p == 3) {
            $scope.isCollapsedDelayed = !$scope.isCollapsedDelayed;
            $scope.isCollapsedTrack = true;
            $scope.isCollapsedRisk = true;
        }
    }
    $scope.onSuccess = function () {
        $location.path('/objectives');
    }

    $scope.processsearchform = function () {
        // $scope = $scope.$new(true);
        //$scope.loadingImage = true;
        var promise = dataservice.searchgoal($scope.addformData);
        promise
          .success(function (data) {

              //$scope.loadingImage = false;
              $scope.goalList = data;
              //console.log($scope.goalList);

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 //$scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {

                 //$scope.loadingImage = false;
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);

         });

    }

    $scope.clearall = function () {

        $scope.searchformData.id = '';
        $scope.searchformData.title = '';
        $scope.searchformData.userteam = '';
        $scope.searchformData.category = $scope.goalcatList[0];
        $scope.searchformData.status = $scope.goalstatusList[0];
        $scope.searchformData.closedObjective = '';
        $scope.searchformData.privateonly = '';
        $scope.searchformData.alignedonly = '';
        $scope.searchformData.closedObjectivecheckbox = false;
        $scope.searchformData.privateonlycheckbox = false;
        $scope.searchformData.alignedonlycheckbox = false;

    }

    $scope.Init = function () {


        $scope.searchformData.id = '';
        $scope.searchformData.title = '';
        $scope.searchformData.userteam = '';
        $scope.searchformData.category = {};
        $scope.searchformData.closedObjective = '';
        $scope.searchformData.privateonly = '';
        $scope.searchformData.alignedonly = '';

        $scope.processsearchform();
        $scope.getgoalstatus();
        $scope.getgoalcat();
        //$scope.GetOnTrackObjectives();
        //$scope.GetRiskObjectives();
        //$scope.GetDelayedObjectives();
        if ($routeParams.query != undefined) {
            $scope.searchformData.title = $routeParams.query;
        }
    }
    $scope.view = "list";
    $scope.changeView = function (view) {
        
        $scope.view = view;
        if (view == "grid")
        {
            console.log(view);
            $timeout(function () {
                $("#accordianObj").zAccordion({
                    slideWidth: "92%",
                    width: "98%",
                    height: 470,
                    auto: false,
                    slideClass:"sliderbody"
                });
                $(".scrollerPanel").niceScroll({
                    cursorcolor: "#cfcfcf",
                    cursorwidth: "8"
                });
            }, 1000);

        }
    }
    $scope.getViewTemp = function () {
        if ($scope.view == "default")
            return 'DefaultView';
        if ($scope.view == "grid") {
            $(".scrollerPanel").niceScroll({
                cursorcolor: "#cfcfcf",
                cursorwidth: "8"
            });
            return 'GridView';
        }
        if ($scope.view == "list")
            return 'ListView';
    }

    $scope.getgoalcat = function () {

        //$scope.loadingImage = true;
        var promise = dataservice.searchgoalcat();
        promise
          .success(function (data) {

              var allgoal = { "id": "", "title": "All" };
              data.splice(0, 0, allgoal);

              //$scope.loadingImage = false;
              $scope.goalcatList = data;

              $scope.searchformData.category = allgoal;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 //$scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)

         });

    }

    $scope.getgoalstatus = function () {

        //$scope.loadingImage = true;
        var promise = dataservice.searchgoalstatus();
        promise
          .success(function (data) {

              var allgoal = { "id": "", "title": "All" };
              data.splice(0, 0, allgoal);

              //$scope.loadingImage = false;
              $scope.goalstatusList = data;

              $scope.searchformData.status = allgoal;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 //$scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)

         });

    }

    //$scope.GetOnTrackObjectives = function () {
    //    $scope.loadingImage = true;
    //    var promise = dashboardservice.GetOrks('ontrack');
    //    promise
    //        .success(function (data) { 
    //            $scope.OnTrackObjectives = data;
    //        })
    //        .error(function (data, status, headers, config) {
    //            if (status = 400) {
    //                $scope.loadingImage = false;
    //                $scope.ErrorMessage = $scope.parseErrors(data);
    //            }
    //        });
    //};
    //$scope.GetRiskObjectives = function () {
    //    $scope.loadingImage = true;
    //    var promise = dashboardservice.GetOrks('atrisk');
    //    promise
    //        .success(function (data) { 
    //            $scope.AtRiskObjectives = data;
    //        })
    //        .error(function (data, status, headers, config) {
    //            if (status = 400) {
    //                $scope.loadingImage = false;
    //                $scope.ErrorMessage = $scope.parseErrors(data);
    //            }
    //        });
    //};
    //$scope.GetDelayedObjectives = function () {
    //    $scope.loadingImage = true;
    //    var promise = dashboardservice.GetOrks('delayed');
    //    promise
    //        .success(function (data) { 
    //            $scope.DelayedObjectives = data;
    //        })
    //        .error(function (data, status, headers, config) {
    //            if (status = 400) { 
    //                $scope.ErrorMessage = $scope.parseErrors(data);
    //            }
    //        });
    //};

    $scope.Init();

    $scope.editgoal = function (id) {
        $scope.editGoalId = id;
        //$scop
e.selgoal = angular.copy(goal);
        //$location.path('/editgoal/' + id.toString());
    };
    $scope.editisopen = function (objective) {
        swal({
            title: "Are you sure?",
            text: "You can change the status if you need in future.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, " + (objective.isopen ? "close" : "open") + " it!",
            closeOnConfirm: true
        }, function () {
            var promise = null;
            promise = dataservice.editgoalisopen(objective.id, !objective.isopen);

            promise.success(function (data) {
                objective.isopen = !objective.isopen;
                if (!objective.isopen)
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
    $scope.deletegoal = function (objective) {

        swal({
            title: "Are you sure?",
            text: "You will not be able to recover this scope.objective!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: true
        }, function () {

            var promise = dataservice.deletegoal(objective.id);
            promise
              .success(function (data) {
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Objective deleted');
                  $scope.objectiveDeleted($scope.goalList.indexOf(objective));
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }

                 $scope.loadingImage = false;
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
             });
        });
    }
    $scope.objectiveDeleted = function (indx) {
        $scope.goalList.splice(indx, 1);
    }
    $scope.closedObjectiveClicked = function () {

        if ($scope.searchformData.closedObjectivecheckbox) {

            $scope.searchformData.closedObjective = true;
        }
        else {
            $scope.searchformData.closedObjective = '';
        }
    }
    $scope.privateClicked = function () {

        if ($scope.searchformData.privateonlycheckbox) {

            $scope.searchformData.privateonly = true;
        }
        else {
            $scope.searchformData.privateonly = '';
        }
    }

    $scope.alignedClicked = function () {

        if ($scope.searchformData.alignedonlycheckbox) {

            $scope.searchformData.alignedonly = true;
        }
        else {
            $scope.searchformData.alignedonly = '';
        }
    }

    $scope.NavigateToNewGoal = function () {
        $location.path('/goal');
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

    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;

    $scope.range = function () {

        var rangeSize = 5;
        var ps = [];
        var start;
        var end;

        if ($scope.pageCount() > rangeSize) {
            start = $scope.currentPage <= 0 ? 0 : $scope.currentPage - 1;
            end = start + (rangeSize - 1);

            var diff = start + (rangeSize - 1) - $scope.pageCount();

            if (diff > 0) {
                start = start - diff;
                end = end - diff;
            }

        }
        else {
            start = 0;
            end = $scope.pageCount();
        }

        for (var i = start; i <= end; i++) {
            ps.push(i);
        }

        return ps;
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.DisablePrevPage = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.goalList.length / $scope.itemsPerPage) - 1;
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount()) {
            $scope.currentPage++;
        }
    };

    $scope.DisableNextPage = function () {
        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };

    $scope.format = 'MM/dd/yyyy';

    $scope.searchByIdTitle = function (goaldata) {
        if ($scope.searchformData.title == undefined)
            $scope.searchformData.title = '';
        return goaldata.title.indexOf($scope.searchformData.title) >= 0 || goaldata.id.toString().indexOf($scope.searchformData.title) >= 0;
    }
}]);