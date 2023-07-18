using Devameet_CSharp.Models;

namespace Devameet_CSharp.Repository.Impl
{
    public class MeetRepositoryImpl : IMeetRepository
    {
        private readonly DevameetContext _context;

        public MeetRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }
        //criar meet
        public void CreateMeet(Meet meet)
        {
            _context.Meets.Add(meet);
            _context.SaveChanges();

        }

        public void DeleteMeet(int meetId)
        {
            //deleta os objetos da sala de reuniao
            List<MeetObjects> meetObjectsExist = _context.MeetObjects.Where(o => o.MeetId == meetId).ToList();
            //percorrer a lista de objetos da sala de reuniao
            foreach (MeetObjects meetObj in meetObjectsExist)
            {
                //deleta os objetos da sala de reuniao
                _context.MeetObjects.Remove(meetObj);
                _context.SaveChanges();
            }
            Meet meet = _context.Meets.First(m => m.Id == meetId);
            _context.Remove(meet);
            _context.SaveChanges();
        }

        //obter meet por id
        public Meet GetMeetById(int idMeet)
        {
            return _context.Meets.Where(m => m.Id == idMeet).FirstOrDefault();
        }

        // lista da sala de reunioes do user
        public List<Meet> GetMeetByUser(int iduser)
        {
            return _context.Meets.Where(m => m.UserId == iduser).ToList();
        }
        //atualizar o meet no banco
        public void UpdateMeet(Meet meet)
        {
            _context.Meets.Update(meet);
            _context.SaveChanges();
        }
    }
}
