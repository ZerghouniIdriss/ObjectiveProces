using AutoMapper;
using OkrsEntreprise.Api.App_Start;
using OkrsEntreprise.Api.Models;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.Api
{
    public class AutoMapperWebConfig
    {
        public static void Configure()
        {
            ConfigureParentGoalMapping();
            ConfigureAssigneeMapping();
            ConfigureTeamToAssigneeMapping();
            ConfigureUserMapping();
            ConfigureCommentMapping();
            ConfigureGoalMapping();
            ConfigureListGoalMapping();
            ConfigureAutocompleteGoalMapping();
            ConfigureSessionGoalMapping();
            ConfigureGoalCategoryMapping();
            ConfigureGoalStatusMapping();
            ConfigureKeyResultMapping();
            ConfigureOneOnOneSessionMapping();
            ConfigureListOneOnOneSessionMapping();
            ConfigureTeamMapping();
            ConfigureListTeamMapping();
            ConfigureSessionStatusMapping();
            ConfigureSurveyOptionMapping();
            ConfigureSurveyQuestionMapping();
            ConfigureSurveyMapping();
            ConfigurePerformanceEvaluationSessionMapping();
            ConfigureListPerformanceEvaluationSessionMapping();
            ConfigurePerformanceEvaluationSessionStatusMapping();
            ConfigurePerformanceSurveyUserAnswerMapping();
            ConfigureActivityMapping();
            ConfigureListUserMapping();
            ConfigureUserTeamViewModelMapping();
            ConfigureRoleMapping();
            ConfigureUserRoleMapping();
            ConfigureUserViewModelMapping();
            ConfigureTenantMapping();
            ConfigureMapObjectMapping();
            ConfigureRecognitionMapping();
            ConfigureIndicator();
            ConfigureGoalIndicator();
            ConfigureIndicatorValue();
        }

        private static void ConfigureGoalMapping()
        {
            Mapper.CreateMap<Goal, GoalFullLoadViewModel>()
                .ForMember(gm => gm.parent, g => g.MapFrom(s => s.Parent))
                .ForMember(gm => gm.children, g => g.MapFrom(s => s.SubGoals))
                .ForMember(gm => gm.keyresults, g => g.MapFrom(s => s.KeyResults))
                .ForMember(gm => gm.comments, g => g.MapFrom(s => s.Comments))
                .ForMember(gm => gm.users, g => g.MapFrom(s => s.Users))
                .ForMember(gm => gm.teams, g => g.MapFrom(s => s.Teams))
                .ForMember(gm => gm.status, g => g.MapFrom(s => s.GoalStatus))
                .ForMember(gm => gm.category, g => g.MapFrom(s => s.GoalCategory))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title))
                .ForMember(gm => gm.description, g => g.MapFrom(s => s.Detail))
                .ForMember(gm => gm.progress, g => g.MapFrom(s => s.Progress))
                .ForMember(gm => gm.duedate, g => g.MapFrom(s => s.Deadline))
                .ForMember(gm => gm.recognitions, g => g.MapFrom(s => s.Recognitions))
                .ForMember(gm => gm.priority, g => g.MapFrom(s => s.Priority))
                .ForMember(gm => gm.GoalIndicators, g => g.MapFrom(s => s.GoalIndicators));

            Mapper.CreateMap<Goal, GoalViewModel>()
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title))
                .ForMember(gm => gm.description, g => g.MapFrom(s => s.Detail))
                .ForMember(gm => gm.progress, g => g.MapFrom(s => s.Progress))
                .ForMember(gm => gm.duedate, g => g.MapFrom(s => s.Deadline))
                .ForMember(gm => gm.priority, g => g.MapFrom(s => s.Priority));


            Mapper.CreateMap<GoalFullLoadViewModel, Goal>()
                .ForMember(gm => gm.Id, g => g.MapFrom(s => s.id))
                .ForMember(gm => gm.KeyResults, g => g.MapFrom(s => s.keyresults))
                .ForMember(gm => gm.Comments, g => g.MapFrom(s => s.comments));

        }

        private static void ConfigureListGoalMapping()
        {
            Mapper.CreateMap<Goal, ListGoalFullLoadViewModel>()
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.users, g => g.MapFrom(s => s.Users))
                .ForMember(gm => gm.teams, g => g.MapFrom(s => s.Teams))
                .ForMember(gm => gm.status, g => g.MapFrom(s => s.GoalStatus))
                .ForMember(gm => gm.category, g => g.MapFrom(s => s.GoalCategory))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title))
                .ForMember(gm => gm.description, g => g.MapFrom(s => s.Detail))
                .ForMember(gm => gm.duedate, g => g.MapFrom(s => s.Deadline))
                .ForMember(gm => gm.isprivate, g => g.MapFrom(s => s.IsPrivate))
                .ForMember(gm => gm.progress, g => g.MapFrom(s => s.Progress))
                .ForMember(gm => gm.keyresults, g => g.MapFrom(s => s.KeyResults))
                .ForMember(gm => gm.hasRecognition, g => g.MapFrom(s => s.Recognitions.Count > 0))
                .ForMember(gm => gm.priority, g => g.MapFrom(s => s.Priority))
                .ForMember(gm => gm.isaligned, g => g.MapFrom(s => s.Parent != null || s.SubGoals.Count > 0));
        }

        private static void ConfigureAutocompleteGoalMapping()
        {
            Mapper.CreateMap<Goal, AutocompleteGoalViewModel>()
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title));
        }

        private static void ConfigureSessionGoalMapping()
        {
            Mapper.CreateMap<Goal, SessionGoalViewModel>()
                .ForMember(gm => gm.keyresults, g => g.MapFrom(s => s.KeyResults))
                .ForMember(gm => gm.status, g => g.MapFrom(s => s.GoalStatus))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title))
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.progress, g => g.MapFrom(s => s.Progress));

            Mapper.CreateMap<SessionGoalViewModel, Goal>()
                //.ForMember(gm => gm.GoalStatus, g => g.MapFrom(s => s.status))
                .ForMember(gm => gm.Id, g => g.MapFrom(s => s.id));

        }

        private static void ConfigureParentGoalMapping()
        {
            Mapper.CreateMap<Goal, ParentChildGoalViewModel>()
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.Title));
        }

        private static void ConfigureGoalCategoryMapping()
        {
            Mapper.CreateMap<GoalCategory, GoalCategoryViewModel>()
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.CategoryTitle));
        }

        private static void ConfigureGoalStatusMapping()
        {
            Mapper.CreateMap<GoalStatus, GoalStatusViewModel>()
                .ForMember(gm => gm.id, g => g.MapFrom(s => s.Id))
                .ForMember(gm => gm.title, g => g.MapFrom(s => s.StatusTitle));
        }


        private static void ConfigureKeyResultMapping()
        {
            Mapper.CreateMap<KeyResult, KeyResultViewModel>()
                .ForMember(k => k.id, kvm => kvm.MapFrom(s => s.Id))
                .ForMember(k => k.title, kvm => kvm.MapFrom(s => s.Title))
                .ForMember(k => k.status, kvm => kvm.MapFrom(s => s.Status));

            Mapper.CreateMap<KeyResultViewModel, KeyResult>()
                .ForMember(k => k.Id, kvm => kvm.MapFrom(s => s.id));
        }

        private static void ConfigureUserMapping()
        {
            Mapper.CreateMap<ApplicationUser, RegisterBindingModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.Password, u => u.MapFrom(s => s.PasswordHash))
                .ForMember(au => au.ConfirmPassword, u => u.MapFrom(s => s.PasswordHash))
                .ForMember(au => au.FName, u => u.MapFrom(s => s.FirstName))
                .ForMember(au => au.LName, u => u.MapFrom(s => s.LastName))
                .ForMember(au => au.UName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email));
        }

        private static void ConfigureListUserMapping()
        {
            Mapper.CreateMap<ApplicationUser, ListUserViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email))
                //.ForMember(au => au.Roles, u => u.MapFrom(s => s.Roles))
                .ForMember(au => au.Teams, u => u.MapFrom(s => s.Teams));

            Mapper.CreateMap<ApplicationUser, RecognitionUserViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Recognitions, u => u.MapFrom(s => s.Recognitions));
        }

        private static void ConfigureUserViewModelMapping()
        {
            Mapper.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.FirstName, u => u.MapFrom(s => s.FirstName))
                .ForMember(au => au.LastName, u => u.MapFrom(s => s.LastName))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email))
                .ForMember(au => au.ManagerUserName, u => u.MapFrom(s => s.Manager.UserName))
                .ForMember(au => au.Goals, u => u.MapFrom(s => s.Goals))
                .ForMember(au => au.Roles, u => u.MapFrom(s => s.Roles))
                .ForMember(au => au.Teams, u => u.MapFrom(s => s.Teams))
                .ForMember(au => au.Recognitions, u => u.MapFrom(s => s.Recognitions));
            Mapper.CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(au => au.ManagerUserName, u => u.MapFrom(s => s.Manager.UserName));



        }

        private static void ConfigureAssigneeMapping()
        {
            Mapper.CreateMap<ApplicationUser, AssignToViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.name, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.isteam, u => u.UseValue<bool>(false));

            Mapper.CreateMap<AssignToViewModel, ApplicationUser>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.name))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => CommonHelperMethods.ResolveAvatarPath(s.Avatar)));

        }

        private static void ConfigureUserTeamViewModelMapping()
        {
            Mapper.CreateMap<ApplicationUser, UserTeamViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email));


            Mapper.CreateMap<UserTeamViewModel, ApplicationUser>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.Email, u => u.MapFrom(s => s.Email));
        }

        private static void ConfigureTeamToAssigneeMapping()
        {
            Mapper.CreateMap<Team, AssignToViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.name, u => u.MapFrom(s => s.Name))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.isteam, u => u.UseValue<bool>(true));
        }



        private static void ConfigureTeamMapping()
        {
            Mapper.CreateMap<Team, TeamViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.name, u => u.MapFrom(s => s.Name))
                .ForMember(au => au.description, u => u.MapFrom(s => s.Description))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => CommonHelperMethods.ResolveAvatarPathForTeam(s.Avatar)))
                .ForMember(au => au.Recognitions, u => u.MapFrom(s => s.Recognitions));
            //.ForMember(au => au.Users, u => u.MapFrom(s => s.Users))

            Mapper.CreateMap<Recognition, TeamRecognitionViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.giver, u => u.MapFrom(s => s.Giver))
                .ForMember(au => au.goal, u => u.MapFrom(s => s.Goal))
                .ForMember(au => au.text, u => u.MapFrom(s => s.Text));

            Mapper.CreateMap<ApplicationUser, RecognitionReceiverViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => CommonHelperMethods.ResolveAvatarPath(s.Avatar)));

            Mapper.CreateMap<Goal, RecognitionGoalViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.title, u => u.MapFrom(s => s.Title));

            Mapper.CreateMap<TeamViewModel, Team>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.id))
                .ForMember(au => au.Name, u => u.MapFrom(s => s.name))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                .ForMember(au => au.Description, u => u.MapFrom(s => s.description))
                .ForMember(au => au.Users, u => u.MapFrom(s => s.Users))
                ;

        }

        private static void ConfigureListTeamMapping()
        {
            Mapper.CreateMap<Team, ListTeamViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.name, u => u.MapFrom(s => s.Name))
                .ForMember(au => au.description, u => u.MapFrom(s => s.Description))
                .ForMember(au => au.Avatar, u => u.MapFrom(s => s.Avatar))
                ;
        }

        private static void ConfigureRoleMapping()
        {
            Mapper.CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.Name, u => u.MapFrom(s => s.Name))
                .ForMember(au => au.Description, u => u.MapFrom(s => s.Description));

            Mapper.CreateMap<RoleViewModel, ApplicationRole>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.Id));

        }

        private static void ConfigureUserRoleMapping()
        {
            Mapper.CreateMap<ApplicationUserRole, RoleViewModel>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.RoleId));
        }
        private static void ConfigureCommentMapping()
        {
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.text, u => u.MapFrom(s => s.Text))
                .ForMember(au => au.createddate, u => u.MapFrom(s => s.CreatedDate))
                .ForMember(au => au.user, u => u.MapFrom(s => s.EntityCreator));

            Mapper.CreateMap<CommentViewModel, Comment>()
                .ForMember(au => au.Id, u => u.MapFrom(s => s.id));

        }

        private static void ConfigureOneOnOneSessionMapping()
        {
            Mapper.CreateMap<OneOnOneSession, OneOnOneSessionViewModel>()
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee))
                .ForMember(s => s.Animator, u => u.MapFrom(s => s.Animator))
                .ForMember(s => s.SessionStatus, u => u.MapFrom(s => s.SessionStatus));

            Mapper.CreateMap<OneOnOneSessionViewModel, OneOnOneSession>()
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee));

        }

        private static void ConfigureListOneOnOneSessionMapping()
        {
            Mapper.CreateMap<OneOnOneSession, ListOneOnOneSessionViewModel>()
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee))
                .ForMember(s => s.Animator, u => u.MapFrom(s => s.Animator))
                .ForMember(s => s.SessionStatus, u => u.MapFrom(s => s.SessionStatus));
        }
        private static void ConfigureSessionStatusMapping()
        {
            Mapper.CreateMap<SessionStatus, OneOnOneSessionStatusViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.title, u => u.MapFrom(s => s.Title));

        }

        private static void ConfigureSurveyOptionMapping()
        {
            Mapper.CreateMap<SurveyOption, SurveyOptionViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.code, u => u.MapFrom(s => s.Code))
                .ForMember(s => s.text, u => u.MapFrom(s => s.Text))
                .ForMember(s => s.order, u => u.MapFrom(s => s.Order))
                .ForMember(s => s.surveyid, u => u.MapFrom(s => s.SurveyId));
        }

        private static void ConfigureSurveyQuestionMapping()
        {
            Mapper.CreateMap<SurveyQuestion, SurveyQuestionViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.code, u => u.MapFrom(s => s.Code))
                .ForMember(s => s.text, u => u.MapFrom(s => s.Text))
                .ForMember(s => s.order, u => u.MapFrom(s => s.Order))
                .ForMember(s => s.surveyid, u => u.MapFrom(s => s.SurveyId));
        }

        private static void ConfigureSurveyMapping()
        {
            Mapper.CreateMap<Survey, SurveyViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.code, u => u.MapFrom(s => s.Code))
                .ForMember(s => s.questions, u => u.MapFrom(s => s.Questions))
                .ForMember(s => s.options, u => u.MapFrom(s => s.Options));

            Mapper.CreateMap<SurveyViewModel, Survey>()
                .ForMember(s => s.Id, u => u.MapFrom(s => s.id))
                .ForMember(s => s.Code, u => u.MapFrom(s => s.code))
                .ForMember(s => s.Questions, u => u.MapFrom(s => s.questions))
                .ForMember(s => s.Options, u => u.MapFrom(s => s.options));

            Mapper.CreateMap<Survey, SurveyViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.code, u => u.MapFrom(s => s.Code))
                .ForMember(s => s.questions, u => u.MapFrom(s => s.Questions))
                .ForMember(s => s.options, u => u.MapFrom(s => s.Options))
                ;
        }


        private static void ConfigurePerformanceEvaluationSessionMapping()
        {
            Mapper.CreateMap<PerformanceEvaluationSession, PerformanceEvaluationSessionViewModel>()
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee))
                .ForMember(s => s.Animator, u => u.MapFrom(s => s.Animator))
                .ForMember(s => s.SessionStatus, u => u.MapFrom(s => s.SessionStatus))
                .ForMember(s => s.Anwers, u => u.MapFrom(s => s.PerformanceSurveyUserAnswers))
                .ForMember(s => s.PerformanceSurvey, u => u.MapFrom(s => s.PerformanceSurvey));

            Mapper.CreateMap<PerformanceEvaluationSessionViewModel, PerformanceEvaluationSession>()
                .ForMember(s => s.Goals, u => u.MapFrom(s => s.Goals))
                .ForMember(s => s.PerformanceSurveyUserAnswers, u => u.MapFrom(s => s.Anwers))
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee));

        }

        private static void ConfigureListPerformanceEvaluationSessionMapping()
        {
            Mapper.CreateMap<PerformanceEvaluationSession, ListPerformanceEvaluationSessionViewModel>()
                .ForMember(s => s.Attendee, u => u.MapFrom(s => s.Attendee))
                .ForMember(s => s.Animator, u => u.MapFrom(s => s.Animator))
                .ForMember(s => s.SessionStatus, u => u.MapFrom(s => s.SessionStatus));
        }

        private static void ConfigurePerformanceEvaluationSessionStatusMapping()
        {
            Mapper.CreateMap<SessionStatus, PerformanceEvaluationSessionStatusViewModel>()
                .ForMember(s => s.id, u => u.MapFrom(s => s.Id))
                .ForMember(s => s.title, u => u.MapFrom(s => s.Title));

        }

        private static void ConfigureActivityMapping()
        {
            Mapper.CreateMap<Activity, ActivityViewModel>()
                .ForMember(av => av.ActivityDate, a => a.MapFrom(s => s.CreatedDate))
                .ForMember(av => av.Actor, a => a.MapFrom(s => s.Actor.UserName))
                .ForMember(av => av.Avatar, a => a.MapFrom(s => s.Actor.Avatar))
                .ForMember(av => av.ActionText, a => a.MapFrom(s => s.ActionText))
                .ForMember(av => av.TargetObject, a => a.MapFrom(s => s.Goal.ToString()));
        }



        private static void ConfigurePerformanceSurveyUserAnswerMapping()
        {
            Mapper.CreateMap<PerformanceSurveyUserAnswer, PerformanceSurveyUserAnswerViewModel>()
                .ForMember(s => s.surveyquestionid, u => u.MapFrom(s => s.SurveyQuestionId))
                .ForMember(s => s.surveyoptionid, u => u.MapFrom(s => s.SurveyOptionId))
                .ForMember(s => s.comment, u => u.MapFrom(s => s.Comment));

            Mapper.CreateMap<PerformanceSurveyUserAnswerViewModel, PerformanceSurveyUserAnswer>()
                .ForMember(s => s.SurveyOptionId, u => u.MapFrom(s => s.surveyoptionid))
                .ForMember(s => s.SurveyQuestionId, u => u.MapFrom(s => s.surveyquestionid))
                .ForMember(s => s.Comment, u => u.MapFrom(s => s.comment));

        }


        private static void ConfigureTenantMapping()
        {
            Mapper.CreateMap<Tenant, TenantViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.name, u => u.MapFrom(s => s.Name));



        }

        private static void ConfigureMapObjectMapping()
        {
            Mapper.CreateMap<Goal, GoalMapViewModel>()
                .ForMember(au => au.id, u => u.MapFrom(s => s.Id))
                .ForMember(au => au.title, u => u.MapFrom(s => s.Title))
                .ForMember(au => au.ParentId, u => u.MapFrom(s => s.ParentId))
                .ForMember(au => au.status, u => u.MapFrom(s => s.GoalStatus))
                .ForMember(au => au.priority, u => u.MapFrom(s => s.Priority));

        }


        private static void ConfigureRecognitionMapping()
        {
            Mapper.CreateMap<ApplicationUser, RecognitionReceiverViewModel>()
                .ForMember(r => r.Id, u => u.MapFrom(s => s.Id))
                .ForMember(r => r.UserName, u => u.MapFrom(s => s.UserName))
                .ForMember(r => r.Avatar, u => u.MapFrom(s => CommonHelperMethods.ResolveAvatarPath(s.Avatar)));

            Mapper.CreateMap<Team, RecognitionTeamReceiverViewModel>()
                .ForMember(r => r.id, u => u.MapFrom(s => s.Id))
                .ForMember(r => r.name, u => u.MapFrom(s => s.Name))
                .ForMember(r => r.description, u => u.MapFrom(s => s.Description))
                .ForMember(r => r.Avatar, u => u.MapFrom(s => CommonHelperMethods.ResolveAvatarPath(s.Avatar)));

            Mapper.CreateMap<Goal, RecognitionGoalViewModel>()
                .ForMember(r => r.id, u => u.MapFrom(s => s.Id))
                .ForMember(r => r.title, u => u.MapFrom(s => s.Title));

            Mapper.CreateMap<Recognition, RecognitionViewModel>()
                .ForMember(r => r.id, u => u.MapFrom(s => s.Id))
                .ForMember(r => r.text, u => u.MapFrom(s => s.Text))
                .ForMember(r => r.giver, u => u.MapFrom(s => s.Giver))
                .ForMember(r => r.goal, u => u.MapFrom(s => s.Goal))
                .ForMember(r => r.receivers, u => u.MapFrom(s => s.Receivers))
                .ForMember(r => r.teamReceivers, u => u.MapFrom(s => s.TeamReceivers));

        }

        private static void ConfigureIndicator()
        {
            Mapper.CreateMap<Indicator, IndicatorViewModel>();
            Mapper.CreateMap<IndicatorViewModel, Indicator>();
        }

        private static void ConfigureIndicatorValue()
        {
            Mapper.CreateMap<IndicatorValue, IndicatorValueViewModel>()
                .ForMember(r => r.text, u => u.MapFrom(s => s.Text))
                .ForMember(r => r.value, u => u.MapFrom(s => s.Id));
            Mapper.CreateMap<IndicatorViewModel, IndicatorValue>();
        }


        private static void ConfigureGoalIndicator()
        {
            Mapper.CreateMap<GoalIndicator, GoalIndicatorViewModel>()
                .ForMember(r => r.IndicatorTitle, u => u.MapFrom(s => s.Indicator.Title))
                .ForMember(r => r.ValueText, u => u.MapFrom(s => s.IndicatorValue.Text))
                .ForMember(r => r.ValueId, u => u.MapFrom(s => s.IndicatorValue.Id));

            Mapper.CreateMap<GoalIndicatorViewModel, GoalIndicator>();
        }

    }
}