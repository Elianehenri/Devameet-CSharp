using Devameet_CSharp.Models;

namespace Devameet_CSharp.Repository
{
    public interface IMeetObjectRepository
    {
        void CreateObjectsMeet(List<MeetObjects> meetObjectsNew, int meetId);
        List<MeetObjects> GetMeetObjectsById(int meetid);
    }
}
