using Domain;

namespace Application.TodoItems.Query
{
    public static class QueryExtension
    {
        public static IQueryable<TodoItem> GetFilteredQuery
            (this IQueryable<TodoItem> query, QueryParameter param)
        {
            // qの絞り込み
            if (!string.IsNullOrEmpty(param.q))
            {
                var paramLower = param.q.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower) ||
                    x.Comments.Any(x => x.Content.ToLower().Contains(paramLower)) ||
                    x.CreatedBy!.UserName.ToLower().Contains(paramLower)
                );
            }

            // Nameの絞り込み
            if (!string.IsNullOrEmpty(param.Name))
            {
                var paramLower = param.Name.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower)
                );
            }

            // コメントでの絞り込み
            if (!string.IsNullOrEmpty(param.Comment))
            {
                var paramLower = param.Comment.ToLower();
                query = query.Where(x =>
                    x.Comments.Any(x => x.Content.ToLower().Contains(paramLower))
                );
            }

            // ユーザー名での絞り込み
            if (!string.IsNullOrEmpty(param.UserName))
            {
                var paramLower = param.UserName.ToLower();
                query = query.Where(x =>
                    x.CreatedBy!.UserName.ToLower().Contains(paramLower)
                );
            }

            // ユーザーIDでの絞り込み
            if (!string.IsNullOrEmpty(param.User))
            {
                query = query.Where(x => x.CreatedBy!.Id == param.User);
            }

            // 状態での絞り込み
            if (param.IsComplete != null)
            {
                query = query.Where(x => x.IsComplete == param.IsComplete);
            }

            return query;
        }
    }
}