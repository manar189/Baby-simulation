function createMovie(name, z, h)
figure

N = length(z(1,:));
for k=1:N
    clf
    
    x_k = z(1,k);
    y_k = z(3,k);
    
    plot(x_k, y_k, 'ko', 'LineWidth', 3, 'MarkerSize', 15)
    hold on
    axis([-0.5 0.5 -0.5 0.1]);
%     drawnow
%     pause(0.01)
    movieVector(k) = getframe;
end

movieWriter = VideoWriter(name, 'MPEG-4');
movieWriter.FrameRate = 1/h;
open(movieWriter);
writeVideo(movieWriter, movieVector);
close(movieWriter);

end

