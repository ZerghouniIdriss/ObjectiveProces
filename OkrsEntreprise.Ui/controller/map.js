okrsapp.controller('map', ['$scope', '$location', '$routeParams', 'localdata', 'goalservice', '$filter',
    function ($scope, $location, $routeParams, localdata, goalservice, $filter) {

        $scope.loadingImage = false;

        $scope.searchformData = {};
        $scope.ErrorMessage = '';
        $scope.goalList = [];
        var level = 0;

        function getNestedChildren(arr, ParentId, level1) {
            var out = []
            for (var i in arr) {

                if (arr[i].ParentId == ParentId) {
                    if (ParentId == null) {
                        level1 = 0;
                        arr[i].width = (50) + "%";
                        arr[i].width2 = (50) + "%";
                        arr[i].Level = level1;
                    } else {
                        arr[i].width = (50 - (level1)) + "%";
                        arr[i].width2 = (50 + (level1)) + "%";
                    }                   
                    var goals = getNestedChildren(arr, arr[i].id, level1+1)

                    if (goals.length) {
                        arr[i].Goals = goals
                    }
                    out.push(arr[i])
                }
            }
          
            return out
        }

       



        
        $scope.collapseAll = function () {
            $scope.$broadcast('angular-ui-tree:collapse-all');
        };

        $scope.expandAll = function () {
            $scope.$broadcast('angular-ui-tree:expand-all');
        };
        $scope.processsearchform = function () {

            $scope.loadingImage = true;
            var promise = goalservice.listGoal();
            promise
              .success(function (data) {
                  $scope.loadingImage = false;
                  var level = 0;
                  $scope.goalList = getNestedChildren(data, null, level);
                  console.log($scope.goalList);
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


        $scope.parseErrors = function (response) {
            var errors = [];
            for (var key in response.ModelState) {
                for (var i = 0; i < response.ModelState[key].length; i++) {
                    errors.push(response.ModelState[key][i]);
                }
            }
            return errors.toString();
        }

        $scope.Init = function () {
            $scope.processsearchform();
        }

        $scope.Init();

    }
]);
