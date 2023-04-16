
okrsapp.directive('performancesurveydisplay', ['localdata', 'dataservice', function (localdata, dataservice) {
    return {
        restrict: 'E',
        templateUrl: '/views/performancesurveydisplay.html',
        replace: true,
        transclude: true,
        scope: {
            answers: '=selectedAnswers',
            survey: '=selectedSurvey'
        },
        link: function (scope, element, attrs) {

            scope.$watch('answers', function (newValue, oldValue) {

                if (newValue) {

                    for (var i = 0; i < newValue.length; i++) {
                        for (var j = 0; j < scope.survey.questions.length; j++) {

                            if (scope.survey.questions[j].id == newValue[i].surveyquestionid) {
                                scope.survey.questions[j].surveyoptionid = newValue[i].surveyoptionid;
                                scope.survey.questions[j].comment = newValue[i].comment;

                                break;
                            }
                        }
                    }
                }
            });

            scope.Init = function () {
                scope.ErrorMessage = '';
            };

            scope.Init();
        }
    };
}]);
