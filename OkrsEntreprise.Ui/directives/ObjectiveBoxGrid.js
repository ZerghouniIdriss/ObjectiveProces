okrsapp.directive('objectiveboxgrid', ['localdata', 'dataservice', '$rootScope', function (localdata, dataservice, rootScope) {

    return {
        restrict: 'E',
        templateUrl: '/views/objectiveboxgrid.html',
        replace: true,
        transclude: true,
        scope: {
            objective: '=boxObjective',
            objectiveDeleted: '=deleted',
            idx: '=index'
        },
        link: function (scope, element, attrs) {

            scope.Init = function () {
                
            }

            scope.currentDate = new Date();

            scope.selkr = {};
            scope.seltitle = {};

            scope.showKeymsg = false;

            scope.ClosePopOver = function () {
                $('body').click();
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
                        console.log(data);
                        scope.objective.isopen = !scope.objective.isopen;
                        
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