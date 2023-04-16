okrsapp.service('dataservice', ['$http', 'localdata', function ($http, localdata) {

    //this.dologin = function (formdata) {
    //    //var promise = $http.post('http://localhost:44305/Token', { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }, formdata);
    //    var promise = $http({
    //        method: 'POST', url: '/token', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, data: { username: formdata.username, password: formdata.password, grant_type: formdata.grant_type }, transformRequest: function (obj) {
    //            var str = [];
    //            for (var p in obj)
    //                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    //            return str.join("&");
    //        }
    //    });

    //    return promise;
    //};

    this.doregistration = function (formdata) {
        var promise = $http.post(this.Url('/api/Account/Register', { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }), formdata);
        return promise;
    };

    this.dologin = function (formdata) {
        var loginData = {
            grant_type: formdata.grant_type,
            username: formdata.username,
            password: formdata.password
        };

        var promise = $.ajax({
            type: 'POST',
            url: this.Url('/Token', { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }),
            data: loginData
        });
        return promise;
    }

    this.dologout = function (formdata) {

        var token = 'Bearer ' + formdata;
        $http.defaults.headers.common.Authorization = token;
        var promise = $http.post(this.Url('/api/Account/Logout'));
        return promise;
    }

    this.creategoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Create'), formdata);
        return promise;
    }

    this.editgoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Edit'), formdata);
        return promise;
    }

    this.deletegoal = function (id) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Delete'), id);
        return promise;
    }

    this.searchgoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Search'), formdata);
        return promise;
    }

    this.autocompletegoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/ForAutocomplete'), formdata);
        return promise;
    }


    this.previousgoals = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/PreviousGoals'), formdata);
        return promise;
    }

    this.nextgoals = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/NextGoals'), formdata);
        return promise;
    }

    this.retrievgoal = function (id) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Retrieve'), id);
        return promise;
    }

    this.editgoaltitle = function (id, title) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditTitle'), { "id": id, "title": title });
        return promise;

    }

    this.editgoalprogress = function (id, progress) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditProgress'), { "id": id, "progress": progress });
        return promise;

    }

    this.editgoalstatus = function (id, status) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditStatus'), { "id": id, "status": status });
        return promise;

    }

    this.editgoalpriority = function (id, priority) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditPriority'), { "id": id, "priority": priority });
        return promise;

    }

    this.editgoalcategory = function (id, cat) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditCategory'), { "id": id, "category": cat });
        return promise;

    }

    this.editgoalisprivate = function (id, ispvt) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditIsPrivate'), { "id": id, "isprivate": ispvt });
        return promise;

    }

    this.editgoalisopen = function (id, isopoen) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditIsOpen'), { "id": id, "isopen": isopoen });
        return promise;

    }

    this.editgoalduedate = function (id, dd) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditDueDate'), { "id": id, "duedate": dd });
        return promise;

    }

    this.editgoaldes = function (id, des) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditDes'), { "id": id, "description": des });
        return promise;

    }

    this.editparent = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditPar'), formdata);
        return promise;

    }

    this.editassignees = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditAssignees'), formdata);
        return promise;

    }


    this.addsubgoal = function (id,subid) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/AddSubGoal'), { "parentid": parseInt(id), "id": parseInt(subid) });
        return promise;

    }

    this.deletesubgoal = function (id, subid) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/DeleteSubGoal'), { "parentid": parseInt(id), "id": parseInt(subid) });
        return promise;

    }


    this.assignme = function (formdate) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/AssignMe'), formdate);
        return promise;

    }

    this.removeme = function (formdate) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/RemoveMe'), formdate);
        return promise;

    }

    this.unassign = function (formdate) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/UnassignUser'), formdate);
        return promise;

    }

    this.searchgoalcat = function () {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalCategory/Search'));
        return promise;
    }

    this.searchgoalstatus = function () {

        this.addToken();
        var promise = $http.post(this.Url('/api/GoalStatus/Search'));
        return promise;
    }

    this.searchuser = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/Search'), formdata);
        return promise;
    }

    this.searchUserList = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/UserList'), formdata);
        return promise;

    }

    this.userAssigneeList = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Account/AssigneeList'), formdata);
        return promise;

    }

    this.searchTeamList = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/TeamList'), formdata);
        return promise;

    }

    this.teamAssigneeList = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/AssigneeList'), formdata);
        return promise;

    }

    this.searchteam = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Team/Search'), formdata);
        return promise;
    }

    this.getOneOnOneSession = function (formdata) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/Create'), formdata);
        return promise;
    }

    this.addcomment = function (id, comment) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Comment/AddComment'), { "goalid": id, "text": comment });
        return promise;

    }

    this.deletecomment = function (comment) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Comment/DeleteComment'), { "goalid": comment.goalid, "text": comment.text, "id": comment.id });
        return promise;

    }

    this.editcomment = function (comment,newtext) {

        this.addToken();
        var promise = $http.post(this.Url('/api/Comment/EditComment'), { "goalid": comment.goalid, "text": newtext, "id": comment.id });
        return promise;

    }

    this.addkeyresult = function (id, keyresult) {

        this.addToken();
        var promise = $http.post(this.Url('/api/KeyResult/AddKeyResult'), { "goalid": id, "title": keyresult });
        return promise;

    }

    this.deletekeyresult = function (keyresult) {

        this.addToken();
        var promise = $http.post(this.Url('/api/KeyResult/DeleteKeyResult'), { "goalid": keyresult.goalid, "title": keyresult.title, "id": keyresult.id });
        return promise;

    }

    this.editkeyresult = function (keyresult) {

        this.addToken();
        var promise = $http.post(this.Url('/api/KeyResult/EditKeyResult'), { "goalid": keyresult.goalid, "title": keyresult.title, "id": keyresult.id });
        return promise;

    }

    this.createsession = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/CreateSession'), formdata);
        return promise;
    }

    this.searchSessionGoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/SearchGoals'), formdata);
        return promise;

    }

    this.searchSessions = function () {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/GetSessions'));
        return promise;

    }

    this.getsession = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/OneOnOneSession/GetSession'),id);
        return promise;
    }

    this.getsurvey = function (name) {
        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/GetSurvey'), { code : name });
        return promise;
    }

    this.empEvalToolPreviousGoals = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/PreviousGoals'), formdata);
        return promise;
    }

    this.empEvalToolNextGoals = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/NextGoals'), formdata);
        return promise;
    }

    this.empEvalToolSearchSessionGoal = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/SearchGoals'), formdata);
        return promise;

    }

    this.createEmpEvalSession = function (formdata) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/CreateSession'), formdata);
        return promise;
    }

    this.searchEmpEvalSessions = function () {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/GetSessions'));
        return promise;

    }

    this.deleteEmpEvalSession = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/Delete'), id);
        return promise;

    }


    this.getEmpEvalSession = function (id) {

        this.addToken();
        var promise = $http.post(this.Url('/api/PerformanceEvaluationSession/GetSession'), id);

        return promise;
    }


    this.editgoalindicator = function (id, indicatorId,indicatorValueId) {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/EditGoalIndicator'), { "id": id, "indicatorId": indicatorId, "indicatorValueId": indicatorValueId });
        return promise;

    }

    this.addToken = function () {
        var token = 'Bearer ' + localdata.getauthenticationID().aID;
        $http.defaults.headers.common.Authorization = token;
    }


    this.Url = function (url) {
        var returl = localdata.getUriBase() + url;
        return returl;
    }
}]);