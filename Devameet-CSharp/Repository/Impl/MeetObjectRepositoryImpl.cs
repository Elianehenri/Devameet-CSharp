using Devameet_CSharp.Models;

namespace Devameet_CSharp.Repository.Impl
{
    public class MeetObjectRepositoryImpl : IMeetObjectRepository
    {
        private readonly DevameetContext _context;

        public MeetObjectRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }
        public void CreateObjectsMeet(List<MeetObjects> meetObjectsNew, int meetId)
        {
            //deleta os objetos da sala de reuniao
            List<MeetObjects> meetObjectsExist = _context.MeetObjects.Where(m => m.MeetId == meetId).ToList();
            //percorrer a lista de objetos da sala de reuniao
            foreach (MeetObjects meetObj in meetObjectsExist)
            {
                //deleta os objetos da sala de reuniao
                _context.MeetObjects.Remove(meetObj);
                _context.SaveChanges();
            }
            //percorrer os novos objetos da sala de reuniao
            foreach (MeetObjects meetobj in meetObjectsNew)
            {
                _context.MeetObjects.Add(meetobj);
                _context.SaveChanges();
            }
        }
    }
}
