okrsapp.directive('editobjective', ['$location', '$timeout', 'localdata', 'dataservice', function ($location, $timeout, localdata, dataservice) {

    return {
        restrict: 'E',
        templateUrl: '/views/Goal/editgoal.html',
        replace: true,
        transclude: true,
        scope: {
            GoalId : '=goalId',
        },
        link: function (scope, element, attrs) {

            scope.editformData = {};
            scope.goalcatList = [];
            scope.goalstatusList = [];
            scope.editname = false;
            scope.edthdr = 'h';
            scope.editdes = false;
            scope.oldtitle = '';
            scope.olddes = '';
            scope.oldcategory = {};
            scope.oldstatus = {};
            scope.oldisprivate = false;
            scope.format = 'MM/dd/yyyy';
            scope.ErrorMessage = '';
            scope.goalList = [];
            scope.assigneeList = [];
            scope.teamList = [];
            scope.userList = [];
            scope.NewKeyForm = {};
            var childselect;
            scope.Init = function () {

                scope.getgoalstatus();
                scope.getgoalcat();
                scope.getuser();
                scope.getteam();

                scope.getgoal(scope.GoalId);
                scope.getgoals();

                $('.chart').easyPieChart({
                    barColor: '#34c73b',
                    size: 130
                });

                //var parenselect = jQuery("#parentSelect" + scope.editformData.id).select2({
                //    width: '100%'
                //});
                //var childselect = jQuery("#childrenSelect" + scope.editformData.id).select2({
                //    width: '100%'
                //});
            }

            scope.addnewgoal = function () {
                //$location.path('/editgoal/' + scope.GoalId + '/goal');
            }

            scope.deletegoal = function () {

                swal({
                    title: "Are you sure?",
                    text: "You will not be able to recover this objective!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    var promise = dataservice.deletegoal(scope.GoalId);
                    promise
                      .success(function (data) {
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Objective deleted');
                          $location.path('objectives');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {

                             scope.loadingImage = false;
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                });
            }

            scope.deleteparentgoal = function () {

                swal({
                    title: 'Do you want to delete parent objective - ' + scope.editformData.parent.title,
                    text: "You will have to add it again later if required",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    scope.loadingImage = true;
                    var oldparent = scope.editformData.parent;
                    scope.editformData.parent = null;

                    var promise = dataservice.editparent(scope.editformData);
                    promise
                      .success(function (data) {

                          scope.goalList.push({ "id": parseInt(oldparent.id), "title": oldparent.title });
                          scope.processAutocompleteGoalList();
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Parent objective deleted');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {

                             scope.loadingImage = false;
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }
                         scope.editformData.parent = oldparent;
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                });
            }

            scope.deletechildgoal = function (idx) {

                swal({
                    title: 'Do you want to delete child objective - ' + scope.editformData.children[idx].title,
                    text: "You will have to add it again later if required",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    scope.loadingImage = true;

                    var promise = dataservice.deletesubgoal(scope.editformData.children[idx].parentid, scope.editformData.children[idx].id);
                    promise
                      .success(function (data) {
                          var deletedObject = { "id": parseInt(scope.editformData.children[idx].id), "title": scope.editformData.children[idx].title };
                          scope.goalList.push(deletedObject);

                          scope.editformData.children.splice(idx, 1);
                          scope.processAutocompleteGoalList();
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Child objective deleted');

                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {

                             scope.loadingImage = false;
                             scope.ErrorMessage = scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                     });
                });
            }

            scope.addchildgoal = function () {

                var promise = dataservice.addsubgoal(scope.editformData.id, childselect.val());
                promise
                  .success(function (data) {

                      for (var i = 0; i < scope.goalList.length; i++) {

                          if (scope.goalList[i].id == childselect.val()) {
                              var child = { "parentid": parseInt(scope.editformData.id), "id": parseInt(scope.goalList[i].id), "title": scope.goalList[i].title };
                              scope.editformData.children.push(child);
                          }
                      }

                      scope.processAutocompleteGoalList();
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'Child objective added');
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {

                         scope.loadingImage = false;
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                 });

            }

            scope.getgoals = function () {

                scope.loadingImage = true;
                var promise = dataservice.autocompletegoal(scope.editformData);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalList = data;
                      scope.processAutocompleteGoalList();

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

            scope.getgoal = function (id) {
                scope.loadingImage = true;
                var promise = dataservice.retrievgoal(id);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.editformData = data;
                      scope.editformData.assignees = [];
                      scope.editformData.newprogress = scope.editformData.progress;
                      scope.checkassign();
                      $timeout(function () {
                          $('#overallChart' + scope.editformData.id).data('easyPieChart').update(scope.editformData.progress);
                          jQuery("#parentSelect" + scope.editformData.id).select2({
                                  width: '100%'
                          });
                          childselect = jQuery("#childrenSelect" + scope.editformData.id).select2({
                              width: '100%'
                          });
                          $("#parentSelect"+scope.editformData.id).on("change", function (e) {
                              //console.log(parselect);
                              scope.$apply(function () {

                                  if (e.val == '') {
                                      scope.editformData.parent = null;
                                  }
                                  else {
                                      var newp = { "id": parseInt(e.val), "title": e.added.text };
                                      scope.editformData.parent = newp;
                                  }

                                  scope.loadingImage = true;
                                  var promise = dataservice.editparent(scope.editformData);
                                  promise
                                    .success(function (data) {

                                        if (e.removed.id != '') {
                                            scope.goalList.push({ "id": parseInt(e.removed.id), "title": e.removed.text });
                                        }

                                        scope.processAutocompleteGoalList();
                                        $.Notification.autoHideNotify('success', 'top right', 'Success', 'Parent objective changed');
                                    })
                                   .error(function (data, status, headers, config) {
                                       if (status == 400) {

                                           scope.loadingImage = false;
                                           scope.ErrorMessage = scope.parseErrors(data);
                                       }
                                       else if (status == 404) {
                                           scope.ErrorMessage = 'Requested link not found';
                                       }

                                       parselect.val(e.removed.id).trigger("change");

                                       if (e.removed.id == '') {
                                           scope.editformData.parent = null;
                                       }
                                       else {
                                           var newp = { "id": parseInt(e.removed.id), "title": e.removed.text };
                                           scope.editformData.parent = newp;
                                       }

                                       $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);
                                   });
                              });

                          });

                      }, 3000);
                      scope.processAutocompleteGoalList();
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         $location.path('/objectives');
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                 });

            }

            scope.gettitletemp = function () {
                if (scope.editname) return 'edittitle'; else return 'distitle';
            }

            scope.getuser = function () {

                scope.loadingImage = true;
                var promise = dataservice.userAssigneeList(scope.addformData);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.userList = data;
                      scope.prepareassignees();
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {

                         scope.loadingImage = false;
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);

                 });

            }

            scope.getteam = function () {

                scope.loadingImage = true;
                var promise = dataservice.teamAssigneeList(scope.addformData);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.teamList = data;
                      scope.prepareassignees();
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else if (status == 404) {

                         scope.loadingImage = false;
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);

                 });

            }

            scope.updateprogress = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalprogress(scope.editformData.id, scope.editformData.newprogress);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.editformData.progress = scope.editformData.newprogress;
                      $('#overallChart'+scope.editformData.id).data('easyPieChart').update(scope.editformData.progress);
                      if (scope.editformData.progress == 100) {
                          swal("Good job!", "You have completed the objective. Keep up the good work!!", "success")
                      }
                      else {
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Progress changed to ' + scope.editformData.progress);
                      }
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
                     scope.editformData.newprogress = scope.editformData.progress;
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });
                scope.ClosePopOver();
            }

            scope.ClosePopOver = function () {
                $('body').click();
            }

            scope.edittitle = function () {
                scope.oldtitle = scope.editformData.title;
                scope.editname = true;
            }

            scope.updatetitle = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoaltitle(scope.editformData.id, scope.editformData.title);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.oldtitle = '';
                      scope.editname = false;
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resettitle();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resettitle = function () {
                scope.editformData.title = scope.oldtitle;
                scope.oldtitle = '';
                scope.editname = false;
            }

            scope.getheadertemp = function () {
                if (scope.edthdr == 'h') return 'dishdr';
                else if (scope.edthdr == 's') return 'edtstus';
                else if (scope.edthdr == 'c') return 'edtcat';
                else if (scope.edthdr == 'p') return 'edtpvt';
                else if (scope.edthdr == 'd') return 'edtdd';
            }

            scope.editstatus = function () {
                scope.oldstatus = scope.editformData.status;

                for (var i = 0; i < scope.goalstatusList.length; i++) {

                    if (scope.goalstatusList[i].id == scope.oldstatus.id) {
                        scope.editformData.status = scope.goalstatusList[i];
                    }
                }

                scope.edthdr = 's';
            }

            scope.updatestus = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalstatus(scope.editformData.id, scope.editformData.status);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.oldstatus = {};
                      scope.edthdr = 'h';
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resetstus();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resetstus = function () {
                scope.editformData.status = scope.oldstatus;
                scope.oldstatus = {};
                scope.edthdr = 'h';
            }

            scope.editcategory = function () {
                scope.oldcategory = scope.editformData.category;

                for (var i = 0; i < scope.goalcatList.length; i++) {

                    if (scope.goalcatList[i].id == scope.oldcategory.id) {
                        scope.editformData.category = scope.goalcatList[i];
                    }
                }

                scope.edthdr = 'c';
            }

            scope.updatecat = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalcategory(scope.editformData.id, scope.editformData.category);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.oldcategory = {};
                      scope.edthdr = 'h';
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resetcat();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resetcat = function () {
                scope.editformData.category = scope.oldcategory;
                scope.oldcategory = {};
                scope.edthdr = 'h';
            }

            scope.editisprivate = function () {
                scope.oldisprivate = scope.editformData.isprivate;

                for (var i = 0; i < scope.privateoptions.length; i++) {

                    if (scope.privateoptions[i].val == scope.oldisprivate) {
                        scope.editformData.isprivate = scope.privateoptions[i];
                    }
                }


                scope.edthdr = 'p';
            }

            scope.privateoptions = [{ label: 'Public', val: false }, { label: 'Private', val: true }];

            scope.updatepvt = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalisprivate(scope.editformData.id, scope.editformData.isprivate.val);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.oldisprivate = false;
                      scope.editformData.isprivate = scope.editformData.isprivate.val;
                      scope.edthdr = 'h';
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {

                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resetpvt();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }

                     scope.loadingImage = false;
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resetpvt = function () {
                scope.editformData.isprivate = scope.oldisprivate;
                scope.oldisprivate = false;
                scope.edthdr = 'h';
            }

            scope.editduedate = function () {
                scope.oldduedate = scope.editformData.duedate;
                scope.edthdr = 'd';
            }

            scope.updateduedate = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoalduedate(scope.editformData.id, scope.editformData.duedate);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.oldduedate = false;
                      scope.edthdr = 'h';
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resetduedate();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resetduedate = function () {
                scope.editformData.duedate = scope.oldduedate;
                scope.oldduedate = '';
                scope.edthdr = 'h';
            }

            scope.getdestemp = function () {
                if (scope.editdes) return 'edtdes'; else return 'disdes';
            }

            scope.editdest = function () {
                scope.olddes = scope.editformData.description;
                scope.editdes = true;
            }

            scope.updatedes = function () {

                scope.loadingImage = true;
                var promise = dataservice.editgoaldes(scope.editformData.id, scope.editformData.description);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.olddes = '';
                      scope.editdes = false;
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.loadingImage = false;
                         //console.log(data);
                         scope.ErrorMessage = scope.parseErrors(data);
                         scope.resetdes();
                     }
                     else if (status == 404) {
                         scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                 });

            }

            scope.resetdes = function () {
                scope.editformData.description = scope.olddes;
                scope.olddes = '';
                scope.editdes = false;
            }

            scope.getgoalcat = function () {

                scope.loadingImage = true;
                var promise = dataservice.searchgoalcat();
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalcatList = data;

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

            scope.getgoalstatus = function () {

                scope.loadingImage = true;
                var promise = dataservice.searchgoalstatus();
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalstatusList = data;
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

            scope.parseErrors = function (response) {
                var errors = [];
                for (var key in response.ModelState) {
                    for (var i = 0; i < response.ModelState[key].length; i++) {
                        errors.push(response.ModelState[key][i]);
                    }
                }
                return errors.toString();
            }

            scope.prepareassignees = function () {
                scope.assigneeList = [];
                scope.assigneeList.push({
                    name: '<strong>Assignees</strong>',
                    msGroup: true
                });

                if (scope.userList.length > 0) {

                    scope.assigneeList.push({
                        name: '<strong>Persons</strong>',
                        msGroup: true
                    });

                    angular.forEach(scope.userList, function (value, key) {

                        scope.assigneeList.push({
                            icon: '<img  src="' + value.Avatar + '" />',
                            name: value.name,
                            id: value.id,
                            email: value.Email,
                            Avatar: value.Avatar,
                            isteam: false,
                            ticked: false
                        });
                    });

                    scope.assigneeList.push({
                        msGroup: false
                    });
                }

                if (scope.teamList.length > 0) {

                    scope.assigneeList.push({
                        name: '<strong>Teams</strong>',
                        msGroup: true
                    });

                    angular.forEach(scope.teamList, function (value, key) {

                        scope.assigneeList.push({
                            icon: '<img  src="' + value.Avatar + '" />',
                            name: value.name,
                            id: value.id,
                            email: '',
                            Avatar: value.Avatar,
                            isteam: true,
                            ticked: false
                        });
                    });

                    scope.assigneeList.push({
                        msGroup: false
                    });
                }

                scope.assigneeList.push({
                    msGroup: false
                });

            }

            scope.assigeensto = function () {

                if (scope.editformData.assignees != null && scope.editformData.assignees.length > 0) {

                    scope.loadingImage = true;
                    var promise = dataservice.editassignees(scope.editformData);
                    promise
                      .success(function (data) {

                          for (var i = 0; i < scope.editformData.assignees.length; i++) {

                              //for (var j = 0; j < scope.assigneeList.length; j++) {
                              //    if (scope.editformData.assignees[i].id == scope.assigneeList[j].id && scope.editformData.assignees[i].isteam == scope.assigneeList[j].isteam) {

                              //        scope.editformData.assignees[i].name = scope.assigneeList[j].name;
                              //        break;
                              //    }
                              //}

                              if (scope.editformData.assignees[i].isteam) {

                                  scope.editformData.teams.push(scope.editformData.assignees[i]);
                              }
                              else {
                                  scope.editformData.users.push(scope.editformData.assignees[i]);
                              }
                          }

                          angular.forEach(scope.assigneeList, function (value, key) {
                              value.ticked = false;
                          });

                          scope.editformData.assignees = [];
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Assignees added');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             scope.loadingImage = false;
                             //console.log(data);
                             scope.ErrorMessage = scope.parseErrors(data);
                             scope.resetdes();
                         }
                         else if (status == 404) {
                             scope.ErrorMessage = 'Requested link not found';
                         }

                         $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)

                     });
                }
            }

            scope.currentUser = function (item) {
                return localdata.getuserData().un.userName == item.user.name;
            }

            scope.addcomment = function () {
                if (scope.NewUserComment != undefined && scope.NewUserComment != '') {

                    scope.loadingImage = true;
                    var promise = dataservice.addcomment(scope.editformData.id, scope.NewUserComment);
                    promise
                      .success(function (data) {

                          scope.loadingImage = false;

                          data.user = { "id": '', "name": localdata.getuserData().un.userName, "Email": '', "Avatar": localdata.getuserData().un.userImage, "isteam": false }
                          scope.editformData.comments.push(data);

                          scope.NewUserComment = '';
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Comment added');
                          //console.log(data);
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

            scope.selcom = {};

            scope.editcomment = function (com) {
                scope.selcom = angular.copy(com);
            }

            scope.resetcomment = function () {
                scope.selcom = {};
            }

            scope.deletecomment = function (indx) {

                swal({
                    title: 'Do you want delete comment - ' + scope.editformData.comments[indx].text,
                    text: "You will have to add it again later if required",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    var promise = dataservice.deletecomment(scope.editformData.comments[indx]);
                    promise
                      .success(function () {

                          scope.loadingImage = false;
                          scope.editformData.comments.splice(indx, 1);
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Comment deleted');
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
                });
            }

            scope.updatecomment = function (indx) {

                var promise = dataservice.editcomment(scope.editformData.comments[indx], scope.selcom.text);
                promise
                  .success(function () {

                      scope.loadingImage = false;
                      scope.editformData.comments[indx].text = scope.selcom.text;
                      scope.resetcomment();
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'Comment changed');
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

            scope.getcomtemp = function (com) {
                if (scope.selcom.id == com.id) return 'editcomment'; else return 'discomment';
            }

            scope.selkr = {};
            scope.showKeymsg = false;
            scope.keyresulttitle = '';

            scope.addkeyresult = function () {
                if (scope.keyresulttitle == undefined || scope.keyresulttitle.length == 0) {
                    scope.showKeymsg = true;
                }
                else {
                    scope.showKeymsg = false;

                    var promise = dataservice.addkeyresult(scope.editformData.id, scope.keyresulttitle);
                    promise
                      .success(function (data) {

                          scope.loadingImage = false;

                          scope.editformData.keyresults.push(data);
                          scope.keyresulttitle = '';
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
                          scope.editformData.keyresults[idx].title = scope.selkr.title;
                          scope.resetkr();
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'keyresult changed');
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

            scope.deletekr = function (idx) {

                swal({
                    title: 'Do you want delete keyresult - ' + scope.editformData.keyresults[idx].title,
                    text: "You will have to add it again later if required",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: true
                }, function () {

                    var promise = dataservice.deletekeyresult(scope.editformData.keyresults[idx]);
                    promise
                      .success(function () {

                          scope.loadingImage = false;
                          scope.editformData.keyresults.splice(idx, 1);
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Keyresult deleted');
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
                });

            }

            scope.isassign = false;

            scope.checkassign = function () {

                scope.isassign = false;

                for (var i = 0; i < scope.editformData.users.length; i++) {

                    if (scope.editformData.users[i].name == localdata.getuserData().un.userName) {
                        scope.isassign = true;
                        break;
                    }
                }
            }

            scope.assigngoal = function () {

                var promise = null;

                if (scope.isassign) {
                    promise = dataservice.removeme(scope.editformData);

                    promise
                      .success(function (data) {

                          scope.loadingImage = false;
                          var idx = -1;

                          for (var i = 0; i < scope.editformData.users.length; i++) {

                              if (scope.editformData.users[i].name == localdata.getuserData().un.userName) {
                                  idx = i;
                              }
                          }

                          scope.editformData.users.splice(idx, 1);
                          scope.checkassign();

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
                else {
                    promise = dataservice.assignme(scope.editformData);

                    promise
                      .success(function (data) {
                          scope.loadingImage = false;
                          var usr = { "id": 0, "name": localdata.getuserData().un.userName, "Avatar": localdata.getuserData().un.userImage, "isteam": false }
                          scope.editformData.users.push(usr);
                          scope.checkassign();
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

            scope.editisopen = function () {
                swal({
                    title: "Are you sure?",
                    text: "You can change the status if you need in future.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, " + (scope.editformData.isopen ? "close" : "open") + " it!",
                    closeOnConfirm: true
                }, function () {
                    var promise = null;
                    promise = dataservice.editgoalisopen(scope.editformData.id, !scope.editformData.isopen);

                    promise.success(function (data) {
                        scope.editformData.isopen = !scope.editformData.isopen;
                    })
                        .error(function (data, status, headers, config) {
                            if (status == 400) {
                                scope.loadingImage = false;
                                scope.ErrorMessage = scope.parseErrors(data);
                            } else if (status == 404) {

                                scope.loadingImage = false;
                                scope.ErrorMessage = 'Requested link not found';

                            }
                            $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage)
                        });
                });
            }

            scope.processAutocompleteGoalList = function () {

                if (scope.editformData.parent != null) {
                    scope.RemoveFromArray(scope.goalList, scope.editformData.parent.id);
                }

                if (scope.editformData.children != undefined && scope.editformData.children != null) {
                    for (var j = 0; j < scope.editformData.children.length; j++) {
                        scope.RemoveFromArray(scope.goalList, scope.editformData.children[j].id);
                    }
                }
            }

            scope.RemoveFromArray = function (array, value) {

                for (var i = 0; i < array.length; i++) {

                    if (array[i].id == value) {
                        array.splice(i, 1);
                        break;
                    }
                }
            }

            scope.UnassignUser = function (user, idx) {

                var inputdata = null;
                if (user.isteam) {
                    inputdata = { "id": scope.editformData.id, "teams": [user], "users": [] };
                }
                else {
                    inputdata = { "id": scope.editformData.id, "teams": [], "users": [user] };
                }

                var promise = dataservice.unassign(inputdata);

                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.ClosePopOver();

                      if (user.isteam) {
                          scope.editformData.teams.splice(idx, 1);
                      }
                      else {
                          scope.editformData.users.splice(idx, 1);
                      }

                      scope.checkassign();

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

            scope.timeAgo = function (time) {
                return $.timeago(time);
            }


            scope.Init();

        }
    }
}]);