using Dapper;
using Microsoft.Data.SqlClient;
using MyofficeApi.Models;
using System.Data;

namespace MyofficeApi.Repositories
{
    public class MyofficeAcpdRepository : IMyofficeAcpdRepository
    {
        private readonly string _connectionString;

        public MyofficeAcpdRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("找不到連線字串");
        }

        public async Task<IEnumerable<MyOfficeAcpd>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM MyOffice_ACPD";
            return await connection.QueryAsync<MyOfficeAcpd>(sql);
        }

        public async Task<MyOfficeAcpd?> GetByIdAsync(string id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM MyOffice_ACPD WHERE ACPD_SID = @Id";
            return await connection.QuerySingleOrDefaultAsync<MyOfficeAcpd>(sql, new { Id = id });
        }

        public async Task<string> CreateAsync(MyOfficeAcpd entity)
        {
            using var connection = new SqlConnection(_connectionString);

            // 1. 呼叫 NEWSID 預存程序產生 20 碼主鍵
            var p = new DynamicParameters();
            p.Add("@TableName", "MyOffice_ACPD");
            p.Add("@ReturnSID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

            await connection.ExecuteAsync("NEWSID", p, commandType: CommandType.StoredProcedure);
            var newSid = p.Get<string>("@ReturnSID");
            entity.ACPD_SID = newSid;

            // 2. 執行 Insert 新增資料
            var sql = @"
                INSERT INTO MyOffice_ACPD 
                (ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Sname, ACPD_Email, ACPD_Status, ACPD_Stop, 
                 ACPD_StopMemo, ACPD_LoginID, ACPD_LoginPWD, ACPD_Memo, ACPD_NowDateTime, ACPD_NowID, ACPD_UPDDateTime, ACPD_UPDID) 
                VALUES 
                (@ACPD_SID, @ACPD_Cname, @ACPD_Ename, @ACPD_Sname, @ACPD_Email, @ACPD_Status, @ACPD_Stop, 
                 @ACPD_StopMemo, @ACPD_LoginID, @ACPD_LoginPWD, @ACPD_Memo, GETDATE(), @ACPD_NowID, GETDATE(), @ACPD_UPDID)";

            await connection.ExecuteAsync(sql, entity);
            return newSid;
        }

        public async Task<bool> UpdateAsync(string id, MyOfficeAcpd entity)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE MyOffice_ACPD SET 
                    ACPD_Cname = @ACPD_Cname, 
                    ACPD_Ename = @ACPD_Ename, 
                    ACPD_Email = @ACPD_Email, 
                    ACPD_Status = @ACPD_Status, 
                    ACPD_UPDDateTime = GETDATE(), 
                    ACPD_UPDID = @ACPD_UPDID
                WHERE ACPD_SID = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                entity.ACPD_Cname,
                entity.ACPD_Ename,
                entity.ACPD_Email,
                entity.ACPD_Status,
                entity.ACPD_UPDID,
                Id = id
            });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM MyOffice_ACPD WHERE ACPD_SID = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}