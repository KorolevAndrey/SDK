﻿// Copyright (C) NeoAxis Group Ltd. This is part of NeoAxis 3D Engine SDK.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Engine;
using Engine.Renderer;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.Utils;
using Engine.MathEx;
using EditorBase;
using WeifenLuo.WinFormsUI.Docking;

namespace ExampleAddonCreationREAddon
{
	public class ResourceEditorAddonImpl : ResourceEditorAddon
	{
		[Config( "ExampleAddonCreationREAddon", "addItemToMainMenu" )]
		internal static bool addItemToMainMenu;
		[Config( "ExampleAddonCreationREAddon", "addButtonToToolbar" )]
		internal static bool addButtonToToolbar;
		[Config( "ExampleAddonCreationREAddon", "addItemToContextMenuOfResourcesTree" )]
		internal static bool addItemToContextMenuOfResourcesTree;
		[Config( "ExampleAddonCreationREAddon", "drawTextOnScreen" )]
		internal static bool drawTextOnScreen;
		[Config( "ExampleAddonCreationREAddon", "overrideCameraSettings" )]
		internal static bool overrideCameraSettings;
		[Config( "ExampleAddonCreationREAddon", "addPageToOptions" )]
		internal static bool addPageToOptions;
		[Config( "ExampleAddonCreationREAddon", "cameraWavingAmplitude" )]
		internal static float cameraWavingAmplitude = .05f;
		[Config( "ExampleAddonCreationREAddon", "cameraWavingSpeed" )]
		internal static float cameraWavingSpeed = 1;

		internal ToolStripMenuItem mainMenuItem;
		internal ToolStripButton toolbarButton;
		internal AddonDockingForm dockingForm;

		//

		public override DockContent OnCreateDockingWindowAtLoading( string windowTypeName )
		{
			if( dockingForm == null && windowTypeName == typeof( AddonDockingForm ).ToString() )
			{
				dockingForm = new AddonDockingForm();
				return dockingForm;
			}
			return base.OnCreateDockingWindowAtLoading( windowTypeName );
		}

		public override bool OnInit( out string mainMenuItemText, out Image mainMenuItemIcon )
		{
			mainMenuItemText = Translate( "Example of Add-on Creation" );
			mainMenuItemIcon = Properties.Resources.Addon_16;

			//register fields with Config attribute to add support of serialization
			EngineApp.Instance.Config.RegisterClassParameters( GetType() );

			if( addItemToMainMenu )
				AddItemToMainMenu();
			if( addButtonToToolbar )
				AddButtonToToolbar();

			return true;
		}

		public override void OnMainMenuItemClick()
		{
			base.OnMainMenuItemClick();

			AddonForm form = new AddonForm( this );
			if( form.ShowDialog( ApplicationData.MainForm ) == DialogResult.OK )
			{
				if( form.AddDockingWindow )
					AddDockingWindow();
				else
					RemoveDockingWindow();

				if( addItemToMainMenu )
					AddItemToMainMenu();
				else
					RemoveItemFromMainMenu();

				if( addButtonToToolbar )
					AddButtonToToolbar();
				else
					RemoveButtonFromToolbar();
			}
		}

		public override void OnShowContextMenuOfResourcesTree( ContextMenuStrip menu )
		{
			base.OnShowContextMenuOfResourcesTree( menu );

			if( addItemToContextMenuOfResourcesTree )
			{
				//add separator
				menu.Items.Add( new ToolStripSeparator() );

				//add menu item
				ToolStripMenuItem item = new ToolStripMenuItem( "Add-on Example!", Properties.Resources.Addon_16,
					delegate( object s, EventArgs e )
					{
						MessageBox.Show( "Click!" );
					} );
				menu.Items.Add( item );
			}
		}

		public override void OnShowOptionsDialog( OptionsDialog dialog )
		{
			base.OnShowOptionsDialog( dialog );

			if( addPageToOptions )
				dialog.AddLeaf( new AddonOptionsLeaf() );
		}

		public override void OnUpdateCameraSettings( Camera camera )
		{
			base.OnUpdateCameraSettings( camera );

			if( overrideCameraSettings )
			{
				//camera.ProjectionType = ProjectionTypes.Perspective;
				//camera.NearClipDistance = nearClipDistance;
				//camera.FarClipDistance = farClipDistance;				
				//camera.Fov = fov;

				float time = EngineApp.Instance.Time * cameraWavingSpeed;
				Vec2 offset = new Vec2( MathFunctions.Cos( time ), MathFunctions.Sin( time ) ) * cameraWavingAmplitude;
				camera.FixedUp = ( new Vec3( offset.X, offset.Y, 1 ) ).GetNormalize();

				//need update Position, Direction after updating FixedUp
				camera.Position = camera.Position;
				camera.Direction = camera.Direction;
			}
		}

		public override void OnRenderScreenUI( GuiRenderer renderer )
		{
			base.OnRenderScreenUI( renderer );

			if( drawTextOnScreen )
			{
				renderer.AddText( "Add-on Example!", new Vec2( .5f, .9f ), HorizontalAlign.Center, VerticalAlign.Top,
					new ColorValue( 1, 0, 0 ) );
				renderer.AddQuad( new Rect( .3f, .94f, .7f, .95f ), new ColorValue( 1, 1, 0 ) );
			}
		}

		string Translate( string text )
		{
			return ToolsLocalization.Translate( GetType().Name, text );
		}

		void AddItemToMainMenu()
		{
			RemoveItemFromMainMenu();

			mainMenuItem = new ToolStripMenuItem( "Add-on Example!", null, delegate( object s, EventArgs e )
			{
				MessageBox.Show( "Click!" );
			} );
			ApplicationData.MainMenu.Items.Add( mainMenuItem );
		}

		void RemoveItemFromMainMenu()
		{
			if( mainMenuItem != null )
			{
				ApplicationData.MainMenu.Items.Remove( mainMenuItem );
				mainMenuItem = null;
			}
		}

		void AddButtonToToolbar()
		{
			RemoveButtonFromToolbar();

			toolbarButton = new ToolStripButton( "Add-on Example!", Properties.Resources.Addon_32, delegate( object s, EventArgs e )
			{
				MessageBox.Show( "Click!" );
			} );
			toolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			ApplicationData.Toolbar.Items.Add( toolbarButton );
		}

		void RemoveButtonFromToolbar()
		{
			if( toolbarButton != null )
			{
				ApplicationData.Toolbar.Items.Remove( toolbarButton );
				toolbarButton = null;
			}
		}

		void AddDockingWindow()
		{
			RemoveDockingWindow();

			dockingForm = new AddonDockingForm();
			dockingForm.Show( ApplicationData.DockPanel, DockState.Float );
        }

		void RemoveDockingWindow()
		{
			if( dockingForm != null )
			{
				dockingForm.Close();
				dockingForm.Dispose();
				dockingForm = null;
			}
		}
	}
}
