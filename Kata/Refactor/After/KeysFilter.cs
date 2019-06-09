using System.Collections.Generic;
using System.Linq;

namespace Kata.Refactor.After
{
    public class KeysFilter
    {
        private readonly ISessionService _sessionService;

        public KeysFilter(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public List<string> FilterGoldenKeys(IList<string> marks)
        {
            if (marks == null || marks.Count == 0)
            {
                return new List<string>();
            }
                        
            var goldenKey = _sessionService.Get<List<string>>("GoldenKey");

            marks = ReturnGoldenKeysByValidation(marks);
            
            return marks.Where(mark => goldenKey.Contains(mark) || IsFakeKey(mark)).ToList();
        }
        
        public List<string> FilterNonGoldenKeys(IList<string> marks)
        {
            if (marks == null || marks.Count == 0)
            {
                return new List<string>();
            }
            
            var silverKeys = _sessionService.Get<List<string>>("SilverKey");
            var copperKeys = _sessionService.Get<List<string>>("CopperKey");

            var keys = silverKeys.Union(copperKeys);

            return marks.Where(mark => keys.Contains(mark) || IsFakeKey(mark)).ToList();
        }
        
        private IList<string> ReturnGoldenKeysByValidation(IList<string> marks)
        {
            var golden02Mark = GetGolden02Mark(marks);
            
            foreach (var mark in golden02Mark)
            {
                if (!HasSameSubStringWithGolden01Mark(marks, mark))
                {
                    marks.Remove(mark);
                }
            }

            return marks;
        }

        private bool HasSameSubStringWithGolden01Mark(IList<string> marks, string mark)
        {
            return marks.Any(x => x.StartsWith("GD01") && mark.Substring(4, 6).Equals(x.Substring(4, 6)));
        }
        
        private List<string> GetGolden02Mark(IList<string> marks)
        {
            return marks.Where(x => x.StartsWith("GD02")).ToList();
        }
        
        private bool IsFakeKey(string mark)
        {
            return mark.EndsWith("FAKE");
        }
    }
}