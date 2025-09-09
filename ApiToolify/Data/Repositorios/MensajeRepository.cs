using ApiToolify.Data.Contratos;
using ApiToolify.Models.DTO;
using MongoDB.Driver;

namespace ApiToolify.Data.Repositorios
{
    public class MensajeRepository : IMensaje
    {
        private readonly IMongoCollection<ContactoMensaje> _mensajes;

        public MensajeRepository(IConfiguration config)
        {
            var connectionString = config["MongoSettings:ConnectionString"];
            var databaseName = config["MongoSettings:DatabaseName"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _mensajes = database.GetCollection<ContactoMensaje>("ContactoMessages");
        }

        public async Task<List<ContactoMensaje>> ListarMensajesAsync()
        {
            return await _mensajes.Find(_ => true).ToListAsync();
        }

        public async Task<ContactoMensaje> ObtenerPorIdAsync(string id)
        {
            var filter = Builders<ContactoMensaje>.Filter.Eq(m => m.id, id);
            return await _mensajes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertarMensajeAsync(ContactoMensaje mensaje)
        {
            await _mensajes.InsertOneAsync(mensaje);
        }

        public async Task EliminarMensajeAsync(string id)
        {
            var filter = Builders<ContactoMensaje>.Filter.Eq(m => m.id, id);
            await _mensajes.DeleteOneAsync(filter);
        }
    }
}
