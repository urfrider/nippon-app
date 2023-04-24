import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Login from "./pages/login/Login";
import Dashboard from "./pages/dashboard/Dashboard";
import Users from "./pages/users/Users";
import UserView from "./pages/users/UserView";
import Staff from "./pages/staff/Staff";
import StaffCreate from "./pages/staff/StaffCreate";
import StaffView from "./pages/staff/StaffView";
import SurveyManagement from "./pages/survey/SurveyManagement";
import LocationData from "./pages/location/LocationData";
import CreateSurvey from "./pages/survey/CreateSurvey";
import PrivateRoute from "./components/PrivateRoute";
import SurveyView from "./pages/survey/SurveyView";
import SurveyResult from "./pages/survey/SurveyResult";
import Rewards from "./pages/rewards/Rewards";
import CreateReward from "./pages/rewards/CreateReward";
import Profile from "./pages/profile/Profile";

// Router set up for dashboard
function Router() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/">
          <Route index element={<Login />} />
          <Route
            path="dashboard/"
            element={
              <PrivateRoute>
                <Dashboard />
              </PrivateRoute>
            }
          />
          <Route path="user/">
            <Route
              index
              element={
                <PrivateRoute>
                  <Users />
                </PrivateRoute>
              }
            />
            <Route
              path=":userId"
              element={
                <PrivateRoute>
                  <UserView />
                </PrivateRoute>
              }
            />
          </Route>
          <Route path="staff/">
            <Route
              index
              element={
                <PrivateRoute>
                  <Staff />
                </PrivateRoute>
              }
            />
            <Route
              path="create"
              element={
                <PrivateRoute>
                  <StaffCreate />
                </PrivateRoute>
              }
            />
            <Route
              path=":staffId"
              element={
                <PrivateRoute>
                  <StaffView />
                </PrivateRoute>
              }
            />
          </Route>
          <Route path="survey-management">
            <Route
              index
              element={
                <PrivateRoute>
                  <SurveyManagement />
                </PrivateRoute>
              }
            />
            <Route
              path="result/:surveyId"
              element={
                <PrivateRoute>
                  <SurveyResult />
                </PrivateRoute>
              }
            />
            <Route
              path=":surveyId"
              element={
                <PrivateRoute>
                  <SurveyView />
                </PrivateRoute>
              }
            />
            <Route
              path={"create"}
              element={
                <PrivateRoute>
                  <CreateSurvey />
                </PrivateRoute>
              }
            />
          </Route>
          <Route path="location-data">
            <Route
              index
              element={
                <PrivateRoute>
                  <LocationData />
                </PrivateRoute>
              }
            />
          </Route>
          <Route path="rewards">
            <Route
              index
              element={
                <PrivateRoute>
                  <Rewards />
                </PrivateRoute>
              }
            />
            <Route
              path="create"
              element={
                <PrivateRoute>
                  <CreateReward />
                </PrivateRoute>
              }
            />
          </Route>
          <Route path="profile">
            <Route
              index
              element={
                <PrivateRoute>
                  <Profile />
                </PrivateRoute>
              }
            />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default Router;
