using System;
using System.Collections.Generic;
using System.Text;

namespace FTPClient.Library
{
    public class ItemInfo
    {
        public ItemInfo()
        {

        }
        
        /// <summary>
        /// Gets or sets item name.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets item type.
        /// </summary>
        public ItemType Type { get; set; } = ItemType.Unknown;

        /// <summary>
        /// Gets or sets item size.
        /// </summary>
        public Nullable<long> Size { get; set; } = null;

        /// <summary>
        /// Gets size into string.
        /// </summary>
        /// <param name="displaySizeLimit">
        /// Capacity limit (default is 5).
        /// 0: Bytes, 1: KB, 2: MB, 3: GB, 4: TB
        /// </param>
        /// <returns></returns>
        public string SizeToString(byte displaySizeLimit = 4)
        {
            string[] unit = { "Bytes", "KB", "MB", "GB", "TB" };
            double temp = (double)this.Size.Value;
            int count = 0;

            while (temp >= 1024.0)
            {
                count += 1;
                temp /= 1024.0;

                if ((count > unit.Length - 1) || (count >= displaySizeLimit))
                    break;
            }

            return String.Format("{0} {1}", Math.Round(temp), unit[count]);
        }

        /// <summary>
        /// Gets or sets item date.
        /// </summary>
        public Nullable<DateTime> Date { get; set; } = null;

        /// <summary>
        /// Gets or sets item permission.
        /// </summary>
        public string Permission { get; set; } = null;

    }
}
