okrsapp.directive('objectivebox', ['localdata', 'dataservice', '$rootScope', function (localdata, dataservice, rootScope) {

    return {
        restrict: 'E',
        templateUrl: '/views/objectivebox.html',
        replace: true,
        transclude: true,
        scope: {
            objective: '=boxObjective',
            objectiveDeleted: '=deleted',
            idx: '=index'
        },
        link: function (scope, element, attrs) {

            scope.Init = function () {
                scope.getgoalstatus();
                scope.objective.newprogress = scope.objective.progress;
            }

            scope.autoExpand = function (e) {
                var element = typeof e === 'object' ? e.target : document.getElementById(e);
                var scrollHeight = element.scrollHeight - 10; // replace 60 by the sum of padding-top and padding-bottom
                element.style.height = scrollHeight + "px";
            };

            scope.updateGoalStatus = function () {
                var promise = dataservice.editgoalstatus(scope.objective.id, scope.objective.status);
                promise
                  .success(function (data) {
                      $.Notification.autoHideNotify('success', 'top right', 'Error', '#' + scope.objective.id + ' - ' + scope.objective.title + ' status update to ' + scope.objective.status.title);
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         $scope.ErrorMessage = $scope.parseErrors(data);
                         $scope.resetstus();
                     }
                     else if (status == 404) {
                         $scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                 });

            }

            scope.getgoalstatus = function () {

                scope.loadingImage = true;
                var promise = dataservice.searchgoalstatus();
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalstatusList = data;

                      angular.forEach(scope.goalstatusList, function (value, key) {
                          if (value.id == scope.objective.status.id) {
                              scope.objective.status = value;

                          }
                      });
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.currentDate = new Date();
            scope.objective.duedate = scope.objective.duedate == null?null: new Date(scope.objective.duedate);
            scope.selkr = {};
            scope.seltitle = {};

            scope.showKeymsg = false;

            scope.addkeyresult = function () {
                if (scope.objective.keyresulttitle == undefined || scope.objective.keyresulttitle.length == 0) {
                    scope.showKeymsg = true;
                }
                else {
                    scope.showKeymsg = false;

                    var promise = dataservice.addkeyresult(scope.objective.id, scope.objective.keyresulttitle);
                    promise
                      .success(function (data) {

                          scope.loadingImage = false;

                          scope.objective.keyresults.push(data);
                          scope.objective.keyresulttitle = '';
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Keyresult added');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.loadingImage = false;
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {

                             scope.loadingImage = false;
                             scope.ErrorMessage = 'Requested link not found';

                         }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });

                }
            }

            scope.getkrtemp = function (kr) {

                if (kr.id === scope.selkr.id) return 'kredit';
                else return 'krdis';
            };

            scope.editkr = function (kr) {
                scope.selkr = angular.copy(kr);
            };

            scope.resetkr = function () {
                scope.selkr = {};
            };

            scope.updatekr = function (idx) {

                if (scope.selkr.title == undefined || scope.selkr.title.length == 0) {
                    scope.showKeymsg = true;
                }
                else {

                    scope.showKeymsg = false;

                    var promise = dataservice.editkeyresult(scope.selkr);
                    promise
                      .success(function (data) {

                          scope.loadingImage = false;
                          scope.objective.keyresults[idx].title = scope.selkr.title;
                          scope.resetkr();
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'keyresult changed');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }
                         scope.loadingImage = false;
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                }
            }

            scope.deletekr = function (idx) {

                swal({
                    title: 'Do you want delete keyresult - ' + scope.objective.keyresults[idx].title,
                    text: "You will have to add it again later if required",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    var promise = dataservice.deletekeyresult(scope.objective.keyresults[idx]);
                    promise
                      .success(function () {

                          scope.loadingImage = false;
                          scope.objective.keyresults.splice(idx, 1);
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Keyresult deleted');

                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }
                         scope.loadingImage = false;
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                });

            }

            scope.ClosePopOver = function () {
                $('body').click();
            }

            scope.updateprogress = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalprogress(scope.objective.id, scope.objective.newprogress);

                promise
                  .success(function (data) {
                      rootScope.$broadcast('updateProgress', {update:true});

                      scope.loadingImage = false;
                      scope.objective.progress = scope.objective.newprogress;

                      if (scope.objective.progress == 100) {
                          swal("Good job!", "You have completed the scope.objective. Keep up the good work!!", "success")
                      }
                      else {
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Progress changed to ' + scope.objective.newprogress);
                      }
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }

                     scope.loadingImage = false;
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });
                scope.ClosePopOver();
            }

            scope.updatestus = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalstatus(scope.objective.id, scope.objective.status);
                promise
                  .success(function (data) {
                      scope.loadingImage = false;
                      scope.ErrorMessage = 'Objective status updated';
                      $.Notification.autoHideNotify('success', 'top right', 'Success', scope.ErrorMessage)

                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     scope.loadingImage = false;
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);
                 });
            }
            scope.editisopen = function () {
                swal({
                    title: "Are you sure?",
                    text: "You can change the status if you need in future.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, " + (scope.objective.isopen ? "close" : "open") + " it!",
                    closeOnConfirm: true
                }, function () {
                    var promise = null;
                    promise = dataservice.editgoalisopen(scope.objective.id, !scope.objective.isopen);

                    promise.success(function (data) {
                        scope.objective.isopen = !scope.objective.isopen;
                        rootScope.$broadcast('ObjectiveOpenCloseChanged', scope.objective);
                        
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
                });
            }

            scope.deletegoal = function () {

                swal({
                    title: "Are you sure?",
                    text: "You will not be able to recover this scope.objective!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    var promise = dataservice.deletegoal(scope.objective.id);
                    promise
                      .success(function (data) {
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Objective deleted');
                          scope.objectiveDeleted(scope.idx);
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }

                         scope.loadingImage = false;
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                });
            }

                scope.gettitletemp = function () {
                    if (scope.objective.id === scope.seltitle.id) return 'edittitle';
                    else return 'distitle';
                }

                scope.edittitle = function () {
                    scope.seltitle = angular.copy(scope.objective);
                }

                scope.resettitle = function () {
                    scope.seltitle = {};
                }

                scope.updatetitle = function () {

                    scope.loadingImage = true;
                    var promise = dataservice.editgoaltitle(scope.objective.id, scope.objective.title);
                    promise
                      .success(function (data) {
                          scope.loadingImage = false;
                          scope.resettitle();
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }

                         scope.loadingImage = false;
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                         scope.resettitle();

                     });

                }

                scope.Init();
            }
        }
    }]);