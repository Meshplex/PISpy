import AlarmToggle from "../components/alarmtoggle";
import EventList from "../components/eventlist";
import Navbar from "../components/navbar";
import PictureModule from "../components/pictureModul";
import UserList from "../components/userlist";

function Dashboard() {
  return (
    <div className="min-h-screen bg-gray-900 pt-20 px-4">
      <Navbar />
      <div className="grid grid-cols-3 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <EventList />
        <AlarmToggle />
        <UserList />
        <PictureModule />
      </div>
    </div>
  );
}

export default Dashboard;
