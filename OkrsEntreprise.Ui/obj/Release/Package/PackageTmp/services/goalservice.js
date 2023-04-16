okrsapp.service('goalservice', ['$http', 'localdata', function ($http, localdata) {

    this.listGoal = function () { 
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/GetGoalListMap'));
        return promise; 
    }

    this.GetClosedGoals = function (formdata) { 
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/GetClosedGoals'), formdata);
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


    this.addsubgoal = function (id, subid) {
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

    this.editcomment = function (comment, newtext) {

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

    this.GetTrendingGoals = function () {
        this.addToken();
        var promise = $http.post(this.Url('/api/Goal/MostRecognized'));
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