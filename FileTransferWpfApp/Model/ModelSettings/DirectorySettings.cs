using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelSettings
{
    public class DirectorySettings
    {   /// <summary>
        /// Наименование прибора
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// Не удалять файлы после обработки, если файлы не удаляются то при запуске 
        /// </summary>
        public bool DeleteProcessedFile { get; set; }
        /// <summary>
        /// Игнорирование недоступных папок
        /// </summary>
        public bool IgnoreUnavailableDirectory { get; set; }
        /// <summary>
        /// 0 и отрицательные таймер отключен, иначе используем таймер а не FileSystemWatcher
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Максимальное количество рестартов программы, после который файл добавляется в игнор
        /// </summary>
        public int MaxRestartCount { get; set; }
        /// <summary>
        /// Максимальное количество попыток обработать файл, после которого будет перезапуск FileSystemWatcher для текущей папки
        /// </summary>
        public int MaxTries { get; set; }
        /// <summary>
        /// Маска для фильтрации файлов
        /// </summary>
        public string? FileFilterMask { get; set; }
        /// <summary>
        /// Путь к папке для мониторинга
        /// </summary>
        public string? MoveFromPath { get; set; }
        /// <summary>
        /// Путь для передачи файлов
        /// </summary>
        public string[]? MoveToPaths { get; set; }
    }
}
