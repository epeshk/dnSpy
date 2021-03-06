﻿/*
    Copyright (C) 2014-2016 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnSpy.Contracts.Hex.Files {
	/// <summary>
	/// Creates and removes <see cref="HexBufferFile"/>s from a <see cref="HexBuffer"/>
	/// </summary>
	public abstract class HexBufferFileService {
		/// <summary>
		/// Constructor
		/// </summary>
		protected HexBufferFileService() { }

		/// <summary>
		/// Gets the buffer
		/// </summary>
		public abstract HexBuffer Buffer { get; }

		/// <summary>
		/// Gets all files
		/// </summary>
		public abstract IEnumerable<HexBufferFile> Files { get; }

		/// <summary>
		/// Creates a file. Overlapping files isn't supported.
		/// </summary>
		/// <param name="span">Span of file</param>
		/// <param name="name">Name</param>
		/// <param name="filename">Filename if possible, otherwise any name</param>
		/// <param name="tags">Tags, see eg. <see cref="PredefinedBufferFileTags"/></param>
		/// <returns></returns>
		public HexBufferFile CreateFile(HexSpan span, string name, string filename, string[] tags) =>
			CreateFiles(new BufferFileOptions(span, name, filename, tags)).Single();

		/// <summary>
		/// Creates files. Overlapping files isn't supported.
		/// </summary>
		/// <param name="options">File options</param>
		/// <returns></returns>
		public abstract HexBufferFile[] CreateFiles(params BufferFileOptions[] options);

		/// <summary>
		/// Removes all files
		/// </summary>
		public void RemoveAllFiles() => RemoveFiles(HexSpan.FullSpan);

		/// <summary>
		/// Removes all files overlapping with <paramref name="span"/>
		/// </summary>
		/// <param name="span">Span</param>
		public abstract void RemoveFiles(HexSpan span);

		/// <summary>
		/// Removes a file
		/// </summary>
		/// <param name="file">File to remove</param>
		public abstract void RemoveFile(HexBufferFile file);

		/// <summary>
		/// Removes files
		/// </summary>
		/// <param name="files">Files to remove</param>
		public abstract void RemoveFiles(IEnumerable<HexBufferFile> files);

		/// <summary>
		/// Raised after files are added
		/// </summary>
		public abstract event EventHandler<BufferFilesAddedEventArgs> BufferFilesAdded;

		/// <summary>
		/// Raised after files are removed
		/// </summary>
		public abstract event EventHandler<BufferFilesRemovedEventArgs> BufferFilesRemoved;

		/// <summary>
		/// Finds a file
		/// </summary>
		/// <param name="position">Position</param>
		/// <returns></returns>
		public abstract HexBufferFile GetFile(HexPosition position);
	}

	/// <summary>
	/// <see cref="HexBufferFile"/> options
	/// </summary>
	public struct BufferFileOptions {
		/// <summary>
		/// true if this is a default instance that hasn't been initialized
		/// </summary>
		public bool IsDefault => Name == null;

		/// <summary>
		/// Span of file
		/// </summary>
		public HexSpan Span { get; }

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Filename if possible, otherwise any name
		/// </summary>
		public string Filename { get; }

		/// <summary>
		/// Tags, see eg. <see cref="PredefinedBufferFileTags"/>
		/// </summary>
		public string[] Tags { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="span">Span of file</param>
		/// <param name="name">Name</param>
		/// <param name="filename">Filename if possible, otherwise any name</param>
		/// <param name="tags">Tags, see eg. <see cref="PredefinedBufferFileTags"/></param>
		public BufferFileOptions(HexSpan span, string name, string filename, string[] tags) {
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));
			if (tags == null)
				throw new ArgumentNullException(nameof(tags));
			Span = span;
			Name = name;
			Filename = filename;
			Tags = tags;
		}
	}

	/// <summary>
	/// <see cref="HexBufferFile"/>s added event args
	/// </summary>
	public sealed class BufferFilesAddedEventArgs : EventArgs {
		/// <summary>
		/// Added files
		/// </summary>
		public HexBufferFile[] Files { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="files">Added files</param>
		public BufferFilesAddedEventArgs(HexBufferFile[] files) {
			if (files == null)
				throw new ArgumentNullException(nameof(files));
			Files = files;
		}
	}

	/// <summary>
	/// <see cref="HexBufferFile"/>s removed event args
	/// </summary>
	public sealed class BufferFilesRemovedEventArgs : EventArgs {
		/// <summary>
		/// Removed files
		/// </summary>
		public HexBufferFile[] Files { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="files">Removed files</param>
		public BufferFilesRemovedEventArgs(HexBufferFile[] files) {
			if (files == null)
				throw new ArgumentNullException(nameof(files));
			Files = files;
		}
	}
}
