using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelDiary.Models.Files
{

/// <summary>
/// Summary description for Thumbnail
/// </summary>
public class Thumbnail
{
	public Thumbnail(string id, byte[] data)
	{
		this.ID = id;
		this.Data = data;
	}


	private string id;
	public string ID
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
		}
	}

	private byte[] thumbnail_data;
	public byte[] Data
	{
		get
		{
			return this.thumbnail_data;
		}
		set
		{
			this.thumbnail_data = value;
		}
	}
	
	
}
}