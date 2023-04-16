okrsapp.controller('recognition', ['$rootScope', '$scope', '$location', '$routeParams', 'localdata', 'recognitionservice', 'dataservice', 'userservice', 'goalservice',
function ($rootScope, $scope, $location, $routeParams, localdata, recognitionservice, dataservice, userservice, goalservice) {

    $scope.loadingImage = false;
    $scope.addformData = {};
    $scope.searchformData = {};
    $scope.ErrorMessage = '';

    $scope.recognitionList = [];
    $scope.mostRecognizedList = [];
    $scope.trendingGoalsList = [];

    $scope.recognitionValue = {};

    $scope.$on('recognitionSelected', function (evnt, val) {
        $scope.recognitionValue = val;
    });

    $scope.$on('RecognitionAdded', function (evnt, val) {
        $scope.recognitionList.splice(0, 0, val);
    });

    $scope.processsearchform = function () {
        return true;
    }

    $scope.GetAllRecognitions = function () {

        var promise = recognitionservice.GetAllRecognitions();
        promise
          .success(function (data) {

              $scope.recognitionList = [];

              angular.forEach(data, function (value, key) {
                  $scope.recognitionList.push(value);
              });
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.ErrorMessage = localdata.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
         });
    }

    $scope.GetMostRecognized = function () {

        var promise = userservice.GetMostRecognized();
        promise
          .success(function (data) {

              $scope.mostRecognizedList = [];

              angular.forEach(data, function (value, key) {
                  $scope.mostRecognizedList.push(value);
              });
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.ErrorMessage = localdata.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
         });
    }

    $scope.GetTrendingGoals = function () {

        var promise = goalservice.GetTrendingGoals();
        promise
          .success(function (data) {

              $scope.trendingGoalsList = [];

              angular.forEach(data, function (value, key) {
                  $scope.trendingGoalsList.push(value);
              });
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.ErrorMessage = localdata.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Requested link not found';
             }
         });
    }


    $scope.Init = function () {
        $scope.GetAllRecognitions();
        $scope.GetMostRecognized();
        $scope.GetTrendingGoals();
    }


    $scope.Init();
}
]);
