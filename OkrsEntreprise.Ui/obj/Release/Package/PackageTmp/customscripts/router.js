okrsapp.config([
    '$routeProvider',
    function ($routeProvider) {

        var checkLogin = function () {
            return [
                '$q', '$location', 'localdata',
                function ($q, $location, localdata) {
                    var deferrer = $q.defer();
                    var _aid = localdata.getauthenticationID();
                    if (_aid.aID == null || _aid.aID == '') {
                        $location.path('/login');
                        deferrer.reject("'/login'");
                    } else {
                        deferrer.resolve();
                    }
                    return deferrer.promise;
                }
            ]
        };

        var checkLoginAgain = function () {
            return [
                '$q', '$location', 'localdata',
                function ($q, $location, localdata) {
                    var deferrer = $q.defer();
                    var _aid = localdata.getauthenticationID();
                    if (_aid != null && (_aid.aID != null || _aid.aID == '')) {
                        $location.path('/dashboard');
                        deferrer.reject("'/dashboard'");
                    } else {
                        deferrer.resolve();
                    }
                    return deferrer.promise;
                }
            ]
        };

        $routeProvider.
        when('/login', {
            templateUrl: 'views/login.html',
            controller: 'login',
            resolve: { checkLoginAgain: checkLoginAgain() }
        }).
        when('/recoverpassword', {
            templateUrl: 'views/recoverpassword.html'
        }).
        when('/dashboard', {
            templateUrl: 'views/dashboard.html',
            controller: 'dashboard',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/teamdashboard', {
            templateUrl: 'views/teamdashboard.html',
            controller: 'teamdashboard',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/companydashboard', {
            templateUrl: 'views/Company/companydashboard.html',
            controller: 'companydashboard',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/objectives', {
            templateUrl: 'views/Goal/goals.html',
            controller: 'goal',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/objectives/:query', {
            templateUrl: 'views/Goal/goals.html',
            controller: 'goal',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/closedobjectives', {
            templateUrl: 'views/Goal/closedgoals.html',
            controller: 'closedgoals',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/goal', {
            templateUrl: 'views/Goal/goal.html',
            controller: 'goal',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editgoal/:goal/:ret', {
            templateUrl: 'views/Goal/goal.html',
            controller: 'goal',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editgoal/:goal', {
            templateUrl: 'views/Goal/editgoal.html',
            controller: 'editgoal',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/register', {
            templateUrl: 'views/register.html',
            controller: 'register',
        }).
        when('/sessions', {
            templateUrl: 'views/OneOnOneSession/sessions.html',
            controller: 'sessions',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/sessiondetail/:id', {
            templateUrl: 'views/OneOnOneSession/sessiondetail.html',
            controller: 'sessiondetail',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/oneOnOneSession', {
            templateUrl: 'views/OneOnOneSession/oneOnOneSession.html',
            controller: 'oneOnOneSession',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/employeesessions', {
            templateUrl: 'views/EmployeeEvaluationSession/employeesessions.html',
            controller: 'employeesessions',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/employeesession', {
            templateUrl: 'views/EmployeeEvaluationSession/employeesession.html',
            controller: 'employeesession',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/employeesessiondetail/:id', {
            templateUrl: 'views/EmployeeEvaluationSession/employeesessiondetail.html',
            controller: 'employeesessiondetail',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/teams', {
            templateUrl: 'views/Team/teams.html',
            controller: 'teams',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/team', {
            templateUrl: 'views/Team/team.html',
            controller: 'team',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editteam/:team', {
            templateUrl: 'views/Team/teamprofile.html',
            controller: 'teamprofile',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/team/:team', {
            templateUrl: 'views/Team/teamprofile.html',
            controller: 'teamprofile',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/tenants', {
            templateUrl: 'views/Tenant/tenants.html',
            controller: 'tenants',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/tenant', {
            templateUrl: 'views/Tenant/tenant.html',
            controller: 'tenants',
            resolve: { checkLoginAgain: checkLogin() }
            }).
        when('/indicators', {
            templateUrl: 'views/Indicator/indicators.html',
            controller: 'indicators',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/indicator', {
            templateUrl: 'views/Indicator/indicator.html',
            controller: 'indicators',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editindicator/:indicator', {
            templateUrl: 'views/Indicator/editIndicator.html',
            controller: 'editindicator',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/recognition', {
            templateUrl: 'views/Recognition/recognitions.html',
            controller: 'recognition',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/categories', {
            templateUrl: 'views/GoalCategory/goalcategories.html',
            controller: 'goalcategories',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/category', {
            templateUrl: 'views/GoalCategory/goalcategory.html',
            controller: 'goalcategories',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editcategory/:category', {
            templateUrl: 'views/GoalCategory/editgoalcategory.html',
            controller: 'editcategory',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/status', {
            templateUrl: 'views/GoalStatus/status.html',
             controller: 'goalstatus',
             resolve: { checkLoginAgain: checkLogin() }
         }).
        when('/newstatus', {
            templateUrl: 'views/GoalStatus/goalstatus.html',
            controller: 'goalstatus',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editstatus/:status', {
            templateUrl: 'views/GoalStatus/editgoalstatus.html',
            controller: 'editstatus',
            resolve: { checkLoginAgain: checkLogin() }
        }).

        when('/users', {
            templateUrl: 'views/User/users.html',
            controller: 'users',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/user', {
            templateUrl: 'views/User/user.html',
            controller: 'users',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        
        //when('/edituser/:user', {
        //    templateUrl: 'views/edituser.html',
        //    controller: 'edituser',
        //    resolve: { checkLoginAgain: checkLogin() }
        //}).
        when('/edituser/:user', {
            templateUrl: 'views/User/userprofile.html',
            controller: 'userprofile',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/company', {
            templateUrl: 'views/Company/company.html',
            controller: 'company',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/surveys', {
            templateUrl: 'views/Survey/surveys.html',
            controller: 'survey',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/survey', {
            templateUrl: 'views/Survey/survey.html',
            controller: 'survey',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/editsurvey/:survey', {
            templateUrl: 'views/Survey/editsurvey.html',
            controller: 'survey',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/activities', {
            templateUrl: 'views/activities.html',
            controller: 'activities',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/inviteusers', {
            templateUrl: 'views/inviteusers.html',
            controller: 'inviteusers',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/masters', {
            templateUrl: 'views/masters.html',
            controller: 'masters',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/map', {
            templateUrl: 'views/map.html',
            controller: 'map',
            resolve: { checkLoginAgain: checkLogin() }
        }).
        when('/error', {
            templateUrl: 'views/error.html',
        }).
        when('/unauthorized', {
            templateUrl: 'views/unauthorized.html',
        }).

        otherwise({
            redirectTo: '/login',
            controller: 'login'
        });
    }
]).
run(function ($rootScope, $location, localdata, managepermission) {

    $rootScope.$on('$routeChangeStart', function (event, next, current) {

        if (!managepermission.isPermitted($location.path())) {
            //console.log(event);
            //event.preventDefault();
            $location.path('/unauthorized');
        }
        //else
        //{

        //}
        //alert($location.path());
    });

    //if (localdata.getuserData().un != undefined)
    //    alert(localdata.getuserData().un.userRoles);
});
