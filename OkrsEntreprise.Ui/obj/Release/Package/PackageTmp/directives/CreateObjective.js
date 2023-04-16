okrsapp.directive('createobjective', ['localdata', 'dataservice', '$rootScope', function (localdata, dataservice, rootScope) {

    return {
        restrict: 'E',
        templateUrl: '/views/createobjective.html',
        replace: true,
        transclude: true,
        scope: {
            lastgoal: '=lastGoalAdded',
            defcat: '=defaultCategory',
            onsuccess: '=onSuccessAdd',
            curentUser: '=assignee'
        },
        link: function (scope, element, attrs) {
            scope.loadingImage = false;
            scope.ErrorMessage = '';
            scope.addformData = {};
            scope.lastgoal = null;
            scope.currentUserObj = {};
            scope.teamList = [];
            scope.userList = [];
            scope.goalcatList = [];
            scope.goalstatusList = [];
            scope.assigneeList = [];
            //scope.defcat = attrs.defaultCategory;
            scope.addnewForm = function () {

                scope.loadingImage = true;
                var promise = dataservice.creategoal(scope.addformData);
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', scope.addformData.title + ' added to database');

                      scope.lastgoal = angular.copy(scope.addformData);
                      scope.lastgoal.id = data.id;
                      scope.InitAddFormData();
                      $('#full-width-modal').modal('toggle');

                      scope.onsuccess(scope.lastgoal);
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

                     scope.lastgoal = null;
                     $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);

                 });

            }

            scope.getform = function () {
                if (scope.formtype == 'more') return 'moredetails'; else return 'basicdetails';
            }

            scope.tomore = function () {
                scope.formtype = 'more';
            }

            scope.tobasic = function () {
                scope.formtype = 'basic';
            }

            scope.getgoalcat = function () {

                scope.loadingImage = true;
                var promise = dataservice.searchgoalcat();
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalcatList = data;

                      //if (scope.goalcatList.length > 0) {
                      //    scope.addformData.category = scope.goalcatList[0];
                      //}
                      scope.setdefaultcategory();
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

            scope.getgoalstatus = function () {

                scope.loadingImage = true;
                var promise = dataservice.searchgoalstatus();
                promise
                  .success(function (data) {

                      scope.loadingImage = false;
                      scope.goalstatusList = data;

                      if (scope.goalstatusList.length > 0) {
                          scope.addformData.status = scope.goalstatusList[0];
                      }
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

            scope.getuser = function () {
                scope.currentUserObj = localdata.getuserData().un;
                if (rootScope.sessionUser != null) {
                    scope.curentUser = rootScope.sessionUser.id;
                } else
                    scope.curentUser = scope.currentUserObj.userId;
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

            scope.addkeyresult = function () {
                if (scope.addformData.keyresulttitle == undefined || scope.addformData.keyresulttitle.length == 0) {
                    scope.showKeymsg = true;
                }
                else {
                    scope.showKeymsg = false;

                    if (scope.addformData.keyresults.indexOf(scope.addformData.keyresulttitle) == -1) {
                        var keyres = { title: scope.addformData.keyresulttitle };
                        scope.addformData.keyresults.push(keyres);
                    }
                    scope.addformData.keyresulttitle = '';
                }
            }

            scope.selkr = {};
            scope.selkr1 = {};


            scope.getkrtemp = function (kr) {
                if (kr.title === scope.selkr.title) return 'kredit';
                else return 'krdis';
            };

            scope.editkr = function (kr) {
                scope.selkr = angular.copy(kr);
                scope.selkr1 = angular.copy(kr);
                //scope.selk1.title = angular.copy(kr);
            };

            scope.resetkr = function () {
                scope.selkr = {};
                scope.selkr1 = {};
                //scope.selkr1.title = '';
            };

            scope.updatekr = function (idx) {
                if (scope.addformData.keyresults.indexOf(scope.selkr1title) == -1 && scope.selkr1.title.length != 0) {
                    scope.addformData.keyresults[idx] = scope.selkr1;
                }
                scope.resetkr();
            }

            scope.deletekr = function (idx) {
                scope.addformData.keyresults.splice(idx, 1);
            }

            scope.InitAddFormData = function () {
                scope.addformData = {};
                scope.addformData.isprivate = false;
                scope.addformData.priority = 0;
                scope.addformData.keyresults = [];
                scope.addformData.assignees = [];

                angular.forEach(scope.assigneeList, function (value, key) {
                    value.ticked = scope.curentUser == value.id ? true : false;
                });
                scope.setdefaultcategory();

                if (scope.goalstatusList.length > 0) {
                    scope.addformData.status = scope.goalstatusList[0];
                }


                scope.formtype = 'basic';
                scope.showKeymsg = false;

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
                            isteam: false,
                            ticked: scope.curentUser == value.id ? true : false,
                        });
                    });

                    scope.assigneeList.push({
                        msGroup: false
                    });
                    //console.log(scope.assigneeList);
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

            scope.setdefaultcategory = function () {

                for (var i = 0; i < scope.goalcatList.length; i++) {
                    if (scope.goalcatList[i].id == scope.defcat) {
                        scope.addformData.category = scope.goalcatList[i];
                    }
                }
            }

            scope.format = 'MM/dd/yyyy';

            scope.Init = function () {
                scope.InitAddFormData();
                scope.getgoalcat();
                scope.getgoalstatus();
                scope.getuser();
                scope.getteam();
            }

            scope.Init();
           
            rootScope.$watch('sessionUser', function () {
                scope.getuser();
            });
        }
    }
}]);