
okrsapp.directive('performancesurvey', ['localdata', 'dataservice', function (localdata, dataservice) {
    return {
        restrict: 'E',
        templateUrl: '/views/performancesurvey.html',
        replace: true,
        transclude: true,
        scope: {
            answers: '=selectedAnswers',
            survey: '=selectedSurvey'
        },
        link: function (scope, element, attrs) {

            scope.Init = function () {
                scope.ErrorMessage = '';
                scope.GetSurvey();
            };

            scope.GetSurvey = function () {

                var promise = dataservice.getsurvey('PERF_EVALUATION_SESSION');

                promise
                  .success(function (data) {
                      scope.survey = data;
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {
                         scope.ErrorMessage = scope.parseErrors(data);
                     }
                     else
                         if (status == 404) {

                             scope.ErrorMessage = 'Requested link not found';
                         }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
                     scope.survey = {};
                 });
            };

            scope.AnswerChanged = function (oid, qid, com) {

                var foundIndex = -1;

                for (var i = 0; i < scope.answers.length; i++) {

                    if (scope.answers[i].surveyquestionid == qid) {
                        foundIndex = i;
                        break;
                    }
                }

                if (foundIndex == -1) {
                    var obj = { surveyquestionid: qid, surveyoptionid: oid, comment: com };
                    scope.answers.push(obj);
                }
                else {
                    scope.answers[foundIndex].surveyoptionid = oid;
                    scope.answers[foundIndex].comment = com;
                }

            };

            scope.CommentChanged = function (qid, com) {

                for (var i = 0; i < scope.answers.length; i++) {

                    if (scope.answers[i].surveyquestionid == qid) {
                        scope.answers[i].comment = com;
                    }
                }

            };

            scope.Init();
        }
    };
}]);
