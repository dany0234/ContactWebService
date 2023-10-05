using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PhoneBookWebService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PhoneBookWebService.Repositories
{
    public class ContactRepository
    {
        private readonly string _connectionString;
        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            var contacts = new List<Contact>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM Contacts", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        while (await reader.ReadAsync())
                        {
                            contacts.Add(new Contact
                            {
                                Id = reader.GetString(reader.GetOrdinal("Id")), // Changed to GetString
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                Birthdate = reader.GetString(reader.GetOrdinal("Birthdate")),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")) 
                            });
                        }
                    }
                }
            }
            return contacts;
        }

        public async Task<Contact> GetContactByIdAsync(string id) // Changed id to string
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SELECT * FROM Contacts WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Contact
                            {
                                Id = reader.GetString(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                Birthdate = reader.GetString(reader.GetOrdinal("Birthdate")),
                                ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task AddContactAsync(Contact contact)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("INSERT INTO Contacts (Id, FirstName, LastName, Address, City, Birthdate, ImagePath) VALUES (@Id, @FirstName, @LastName, @Address, @City, @Birthdate, @ImagePath)", connection))
                {
                    command.Parameters.AddWithValue("@Id", contact.Id);
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@Address", contact.Address);
                    command.Parameters.AddWithValue("@City", contact.City);
                    command.Parameters.AddWithValue("@Birthdate", contact.Birthdate);
                    command.Parameters.AddWithValue("@ImagePath", (object)contact.ImagePath ?? DBNull.Value);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, Address = @Address, City = @City, Birthdate = @Birthdate, ImagePath = @ImagePath WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@Address", contact.Address);
                    command.Parameters.AddWithValue("@City", contact.City);
                    command.Parameters.AddWithValue("@Birthdate", contact.Birthdate);
                    command.Parameters.AddWithValue("@ImagePath", (object)contact.ImagePath ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@Id", contact.Id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteContactAsync(string id) // Changed id to string
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("DELETE FROM Contacts WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
