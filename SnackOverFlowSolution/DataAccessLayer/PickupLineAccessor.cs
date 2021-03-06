﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// Robert Forbes
    /// Created: 
    /// 2017/04/13
    /// 
    /// Class to handle database interactions involving pickup lines.
    /// </summary>
    public static class PickupLineAccessor
    {
        /// <summary>
        /// Robert Forbes
        /// Created: 
        /// 2017/04/13
        /// 
        /// Retrieves pickup lines based on the given pickup id.
        /// </summary>
        /// 
        /// <remarks>
        /// Aaron Usher
        /// Created: 
        /// 2017/04/21
        /// 
        /// Standardized method.
        /// </remarks>
        /// 
        /// <param name="pickupId">The id of the pickup the lines go with.</param>
        /// <returns>List of pickup lines.</returns>
        public static List<PickupLine> RetrievePickupLinesForPickup(int? pickupId)
        {
            var lines = new List<PickupLine>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_pickup_line_from_search";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PICKUP_ID", pickupId);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lines.Add(new PickupLine()
                        {
                            PickupLineId = reader.GetInt32(0),
                            PickupId = reader.GetInt32(1),
                            ProductId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            PickupStatus = reader.GetBoolean(4)
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lines;
        }


        /// <summary>
        /// Robert Forbes
        /// Created: 
        /// 2017/04/19
        /// 
        /// Updates a pickup line in the database.
        /// </summary>
        /// 
        /// <remarks>
        /// Aaron Usher
        /// Updated: 
        /// 2017/04/21
        /// 
        /// Standardized method.
        /// </remarks>
        /// 
        /// <param name="oldPickupLine">The pickup line as it was in the database.</param>
        /// <param name="newPickupLine">The pickup line as it should be.</param>
        /// <returns>Rows affected.</returns>
        public static int UpdatePickupLine(PickupLine oldPickupLine, PickupLine newPickupLine)
        {
            int rows = 0;

            var cmdText = @"sp_update_pickup_line";
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@old_PICKUP_LINE_ID", oldPickupLine.PickupLineId);
            cmd.Parameters.AddWithValue("@old_PICKUP_ID", oldPickupLine.PickupId);
            cmd.Parameters.AddWithValue("@old_PRODUCT_ID", oldPickupLine.ProductId);
            cmd.Parameters.AddWithValue("@old_QUANTITY", oldPickupLine.Quantity);
            cmd.Parameters.AddWithValue("@old_PICK_UP_STATUS", oldPickupLine.PickupStatus);
            cmd.Parameters.AddWithValue("@new_PICKUP_ID", newPickupLine.PickupId);
            cmd.Parameters.AddWithValue("@new_PRODUCT_ID", newPickupLine.ProductId);
            cmd.Parameters.AddWithValue("@new_QUANTITY", newPickupLine.Quantity);
            cmd.Parameters.AddWithValue("@new_PICK_UP_STATUS", newPickupLine.PickupStatus);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        /// <summary>
        /// Robert Forbes
        /// Created: 
        /// 2017/04/19
        /// </summary>
        /// 
        /// <remarks>
        /// Aaron Usher
        /// Updated: 
        /// 2017/04/21
        /// 
        /// Standardized method.
        /// </remarks>
        /// 
        /// <param name="pickupLineId">The id of the pickup line to retrieve.</param>
        /// <returns>The pickup line.</returns>
        public static PickupLine RetrievePickupLine(int? pickupLineId)
        {
            PickupLine line = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_pickup_line";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@PICKUP_LINE_ID", pickupLineId);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    line = new PickupLine()
                    {
                        PickupLineId = reader.GetInt32(0),
                        PickupId = reader.GetInt32(1),
                        ProductId = reader.GetInt32(2),
                        Quantity = reader.GetInt32(3),
                        PickupStatus = reader.GetBoolean(4)
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return line;
        }

        /// <summary>
        /// Ryan Spurgetis
        /// 4/27/2017
        /// 
        /// Retrieves a list of pickups picked up by drivers
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<PickupLineAndProductName> RetrievePickupLinesReceived(bool? status)
        {
            var lines = new List<PickupLineAndProductName>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_pickup_line_pickups_receieved";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PICK_UP_STATUS", status);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lines.Add(new PickupLineAndProductName()
                        {
                            PickupLineId = reader.GetInt32(0),
                            PickupId = reader.GetInt32(1),
                            ProductId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            PickupStatus = reader.GetBoolean(4),
                            ProductName = reader.GetString(5)
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lines;
        }

        /// <summary>
        /// Ryan Spurgetis
        /// 4/29/2017
        /// 
        /// Removes PickupLine from database
        /// </summary>
        /// <param name="pickupLine"></param>
        /// <returns></returns>
        public static int DeletePickupLine(PickupLine pickupLine)
        {
            int result = 0;
            
            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_delete_pickup_line";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PICKUP_LINE_ID", pickupLine.PickupLineId);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Eric Walton
        /// 5/10/2017
        /// </summary>
        /// <param name="pickupLine"></param>
        /// <returns></returns>
        public static int CreatePickupLine(PickupLine pickupLine)
        {
            int pickupLineId = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_create_pickup_line";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PICKUP_ID", pickupLine.PickupId);
            cmd.Parameters.AddWithValue("@PRODUCT_ID", pickupLine.ProductId);
            cmd.Parameters.AddWithValue("@QUANTITY", pickupLine.Quantity);
            cmd.Parameters.AddWithValue("PICK_UP_STATUS", pickupLine.PickupStatus);

            try
            {
                conn.Open();
                //int.TryParse(cmd.ExecuteScalar().ToString(), out orderLineID);
                decimal id = (decimal)cmd.ExecuteScalar();
                pickupLineId = (int)id;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return pickupLineId;
        }

    }
}
