using System.Drawing.Drawing2D;

namespace ScottReece.BorderCompare.App
{
    public partial class MainForm : Form
    {
        public GeoDatabase GeoDb = new();
        private List<GeoArea> _areas = new();

        private Setup _setup = new();
        private Setup _setupB = new();
        private Setup[] _setups;

        public MainForm()
        {
            _setups = [_setup, _setupB];

            InitializeComponent();
            pnlEarthDisplay.Paint += PnlEarthDisplayPaint;
            pnlEarthDisplay.MouseWheel += PnlEarthDisplayOnMouseWheel;
            timer1.Tick += Timer1OnTick;

            pnlEarthDisplay.MouseDown += PnlEarthDisplayOnMouseDown;
            pnlEarthDisplay.MouseUp += PnlEarthDisplayOnMouseUp;
            pnlEarthDisplay.MouseMove += PnlEarthDisplayMouseMove;
            pnlEarthDisplay.MouseLeave += PnlEarthDisplayOnMouseLeave;
            pnlEarthDisplay.DoubleClick += PnlEarthDisplayOnDoubleClick;

            var pos = new EarthPosition();


            pos.Center = new LatLong(32, -112);
            pos.Center = new LatLong(63.588, -154.5);
            pos.ZoomFactor = 1;

            _setup._position = pos;
            _setupB._position = pos;
            _setupB._palette = new Palette()
            {
                BorderPen = new Pen(Color.Magenta, 3){DashStyle = DashStyle.DashDot},
                CountryBrush = null,//new SolidBrush(Color.FromArgb(100, 0, 0, 255)),
                OceanBrush =  null
            };
        }

        private void PnlEarthDisplayOnDoubleClick(object? sender, EventArgs e)
        {
            var form = new OverlayForm(_areas);
        }

        private void PnlEarthDisplayOnMouseLeave(object? sender, EventArgs e)
        {
            EndDragging();
        }

        private void EndDragging()
        {
            foreach (var setup in _setups)
            {
                setup._dragging = false;
            }
        }

        Setup GetSetup(MouseButtons btn) => btn == MouseButtons.Left ? _setup : _setupB;

        private void PnlEarthDisplayMouseMove(object? sender, MouseEventArgs e)
        {
            var setup = GetSetup(e.Button);
            HandleMouseMove(e.Location, setup);
        }

        private void PnlEarthDisplayOnMouseUp(object? sender, MouseEventArgs e)
        {
            var setup = GetSetup(e.Button);
            HandleMouseMove(e.Location, setup);

            EndDragging();
        }

        private void HandleMouseMove(Point p, Setup setup)
        {
            if (!setup._dragging || setup._lastPaintInfo == null)
                return;

            var delta = new Point(p.X - setup._dragStart!.Value.X, p.Y - setup._dragStart!.Value.Y);
            var degreesEW = setup._lastPaintInfo.DegreesPerPixel * delta.X * -1;
            var degreesNS = setup._lastPaintInfo.DegreesPerPixel * delta.Y;

            var newLatLong = new LatLong(setup._startEarthPosition.Center.Latitude + degreesNS, setup._startEarthPosition.Center.Longitude + degreesEW);
            if (newLatLong.Latitude > 90)
                newLatLong.Latitude = 90;
            if (newLatLong.Latitude < -90)
                newLatLong.Latitude = -90;

            var newPosition = new EarthPosition(newLatLong, setup._position.ZoomFactor);
            setup._position = newPosition;
            pnlEarthDisplay.Invalidate();
        }

        private void PnlEarthDisplayOnMouseDown(object? sender, MouseEventArgs e)
        {
            var setup = GetSetup(e.Button);

            setup._dragStart = e.Location;
            setup._startEarthPosition = setup._position;
            setup._dragging = true;
        }

        private void PnlEarthDisplayOnMouseWheel(object? sender, MouseEventArgs e)
        {
            var delta = e.Delta / 120;
            _setup._position.ZoomFactor *= (float)Math.Pow(1.1, delta);
            pnlEarthDisplay.Invalidate();
        }

        private void Timer1OnTick(object? sender, EventArgs e)
        {
            //var lon = ((DateTime.Now.TimeOfDay.TotalSeconds % 10) / 10) * 360;

            //_position.Center = new LatLong(0, lon);
            //panel1.Invalidate();
        }

        private void PnlEarthDisplayPaint(object? sender, PaintEventArgs e)
        {
            _setupB._position.ZoomFactor = _setup._position.ZoomFactor;
            var painter = new EarthPainter();
            _setup._lastPaintInfo = painter.Draw(e.Graphics, pnlEarthDisplay.ClientRectangle, _areas, _setup._position, _setup._palette);
            _setupB._lastPaintInfo = painter.Draw(e.Graphics, pnlEarthDisplay.ClientRectangle, _areas, _setupB._position, _setupB._palette);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadCountries();
        }

        private async Task LoadCountries()
        {
            _areas = await GeoDb.GetAllCountries();
            pnlEarthDisplay.Invalidate();
        }

        private class Setup
        {
            public bool _dragging = false;
            public Point? _dragStart;
            public EarthPosition _startEarthPosition;

            public EarthPosition _position;
            public Palette _palette = new();
            public MapInfo? _lastPaintInfo;
        }
    }
}
