import { NavLink, Route, Routes, useNavigate } from "react-router-dom";
import { useAuth } from "./auth/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import DashboardPage from "./pages/DashboardPage";
import EventDetailPage from "./pages/EventDetailPage";
import EventEditPage from "./pages/EventEditPage";
import EventsPage from "./pages/EventsPage";
import LoginPage from "./pages/LoginPage";
import ParticipantEditPage from "./pages/ParticipantEditPage";
import ParticipantsPage from "./pages/ParticipantsPage";
import ProfilePage from "./pages/ProfilePage";
import RegisterPage from "./pages/RegisterPage";

function App() {
  const auth = useAuth();
  const navigate = useNavigate();

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div>
          <p className="eyebrow">KMC Platform</p>
          <h1>Event Admin</h1>
          <p className="sidebar-copy">
            Role-aware React frontend for event discovery, registration, and organizer workflows.
          </p>
        </div>

        <nav className="nav-links">
          {auth.isAuthenticated ? (
            <NavLink to="/" end>
              Dashboard
            </NavLink>
          ) : null}
          <NavLink to="/events">Events</NavLink>
          {auth.isAuthenticated ? <NavLink to="/profile">Profile</NavLink> : null}
          {auth.isAdmin ? <NavLink to="/participants">Participants</NavLink> : null}
          {!auth.isAuthenticated ? <NavLink to="/login">Login</NavLink> : null}
          {!auth.isAuthenticated ? <NavLink to="/register">Register</NavLink> : null}
        </nav>

        <div className="session-card">
          {auth.isAuthenticated ? (
            <>
              <p className="session-name">{auth.user.fullName}</p>
              <p className="session-role">
                {["Regular", "Organizer", "Admin"][auth.user.role] ?? auth.user.role}
              </p>
              <button
                className="secondary-button"
                onClick={() => {
                  auth.clearSession();
                  navigate("/login");
                }}
                type="button"
              >
                Logout
              </button>
            </>
          ) : (
            <p className="session-role">Browse events publicly or sign in to manage registrations.</p>
          )}
        </div>
      </aside>

      <main className="content">
        <Routes>
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <DashboardPage />
              </ProtectedRoute>
            }
          />
          <Route path="/events" element={<EventsPage />} />
          <Route path="/events/:id" element={<EventDetailPage />} />
          <Route
            path="/events/:id/edit"
            element={
              <ProtectedRoute roles={[1, 2]}>
                <EventEditPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/participants"
            element={
              <ProtectedRoute roles={[2]}>
                <ParticipantsPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/participants/:id/edit"
            element={
              <ProtectedRoute roles={[2]}>
                <ParticipantEditPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/profile"
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            }
          />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
